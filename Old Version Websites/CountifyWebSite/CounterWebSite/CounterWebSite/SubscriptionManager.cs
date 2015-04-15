using CounterWebSite.Mailers;
using CounterWebSite.Models;
using DatabaseContext;
using log4net;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace CounterWebSite
{
    public enum SubscriptionType
    {
        ProductUpdates,
        NewOrders,
        CounterFailures,
        WebSiteErrors
    }

    public class SubscriptionManager
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(SubscriptionManager));
        private CounterDbContext _db;
        //instead of this, create a new mailer for each message (see: https://github.com/smsohan/MvcMailer/issues/96)
        private ICountifyMailer _mailer = new CountifyMailer();

        public SubscriptionManager(CounterDbContext DatabaseContext)
        {
            _db = DatabaseContext;
        }

        public void SeedSubscriptions()
        {
            AddOrUpdateSubscriptionType(
                Type: SubscriptionType.ProductUpdates,
                Description: "Recieve updates on the Countify project.",
                AdminOnly: false,
                RequiresAccount: false);

            AddOrUpdateSubscriptionType(
                Type: SubscriptionType.NewOrders,
                Description: "Recieve an email any time a new order is submitted through the website.",
                AdminOnly: true,
                RequiresAccount: true);

            AddOrUpdateSubscriptionType(
                Type: SubscriptionType.CounterFailures,
                Description: "Receive an email any time a failure is detected with one of the counters.",
                AdminOnly: true,
                RequiresAccount: true);

            AddOrUpdateSubscriptionType(
                Type: SubscriptionType.WebSiteErrors,
                Description: "Receive an email notification when an errors/exceptions occurs on the site.",
                AdminOnly: true,
                RequiresAccount: true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="EmailAddress"></param>
        /// <returns>
        /// False if the subscription already exists for the email address, or
        /// True if the subscription is created successfully
        /// </returns>
        public bool CreateSubscription(SubscriptionType Type, string EmailAddress)
        {
            int subId = GetTypeId(Type);
            EmailAddress = EmailAddress.Trim().ToLower();

            //check if this email address belongs to an existsing user
            UserProfile user = _db.UserProfiles.FirstOrDefault(u => u.Email.Trim().ToLower() == EmailAddress);

            if (user != null && !user.Subscriptions.Any(s => s.SubscriptionTypeId == subId))
            {
                //create a new subscription by userId
                user.Subscriptions.Add(new Subscription()
                {
                    Email = EmailAddress,
                    SubscriptionTypeId = subId,
                    UserId = user.UserId
                });
            }
            else if (user == null && !_db.Subscriptions.Any(s => s.SubscriptionTypeId == subId && s.Email.Trim().ToLower() == EmailAddress))
            {
                //create a new subscription by email
                _db.Subscriptions.Add(new Subscription()
                {
                    Email = EmailAddress,
                    SubscriptionTypeId = subId,
                    UserId = null
                });
            }
            else
            {
                //subscription already exists
                return false;
            }

            _db.SaveChanges();
            return true;
        }

        public bool UpdateSubscriptions(List<int> SubscriptionTypeIds)
        {
            bool admin = Roles.IsUserInRole("Administrators");
            UserProfile user = _db.UserProfiles.Find(WebSecurity.CurrentUserId);
            List<int> userSubs = user.Subscriptions.Select(s => s.SubscriptionTypeId).ToList();
            List<int> newTypeIds = SubscriptionTypeIds.Where(id => !userSubs.Contains(id)).ToList();
            List<Subscription> deleteSubs = user.Subscriptions.Where(sub => !SubscriptionTypeIds.Contains(sub.SubscriptionTypeId)).ToList();

            try
            {
                //create new subscriptions
                foreach (int subId in SubscriptionTypeIds.Where(id => !userSubs.Contains(id)))
                {
                    Subscription sub = new Subscription()
                    {
                        SubscriptionTypeId = subId,
                        UserId = user.UserId,
                    };
                    _db.Subscriptions.Add(sub);
                }

                //delete removed subscriptions
                foreach (Subscription sub in deleteSubs)
                {
                    _db.Subscriptions.Remove(sub);
                }

                _db.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error("Error updating user subscriptions", e);
                Elmah.ErrorSignal.FromCurrentContext().Raise(e, HttpContext.Current);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Associates existing subscriptions with a user profile when the profile is created
        /// </summary>
        /// <param name="UserName"></param>
        public void MigrateSubscriptions(string UserName)
        {
            //get the user profile from the database
            UserProfile user = _db.UserProfiles.FirstOrDefault(u => u.UserName == UserName);
            if (user == null)
            {
                throw new ArgumentException("The user name provided does not exist", "EmailAddress");
            }

            foreach (Subscription sub in _db.Subscriptions.Where(s => s.Email.Trim().ToLower() == user.Email.Trim().ToLower()))
            {
                sub.Email = null;
                sub.UserId = user.UserId;
            }
        }

        public void SendEmails(SubscriptionType SubType, EmailModel EmailModel)
        {
            CreateAndSendMessages(SubType, EmailModel, false);
        }

        //NOTE: removed for not working on the production server
        //public void SendEmailsAsync(SubscriptionType SubType, EmailModel EmailModel)
        //{
        //    CreateAndSendMessages(SubType, EmailModel, true);
        //}

        #region Helpers

        private string GetTypeName(SubscriptionType SubscriptionType)
        {
            return Enum.GetName(typeof(SubscriptionType), SubscriptionType);
        }

        /// <summary>
        /// Throws exception if subscription type does not exist in the database
        /// </summary>
        /// <param name="SubscriptionType"></param>
        /// <returns></returns>
        private int GetTypeId(SubscriptionType SubscriptionType)
        {
            string TypeName = GetTypeName(SubscriptionType);
            return _db.SubscriptionTypes.First(s => s.Name == TypeName).Id;
        }

        private void AddOrUpdateSubscriptionType(SubscriptionType Type, string Description, bool AdminOnly, bool RequiresAccount)
        {
            //check if this type already exists
            string subName = GetTypeName(Type);
            var record = _db.SubscriptionTypes.FirstOrDefault(s => s.Name == subName);
            bool typeExists = record != null;

            //create a new db record if it doesn't exist
            if (!typeExists)
            {
                record = new DatabaseContext.SubscriptionType();
            }

            //update the details for the db record
            record.Name = Enum.GetName(typeof(SubscriptionType), Type);
            record.Description = Description;
            record.AdministratorOnly = AdminOnly;
            record.RequiresAccount = RequiresAccount;

            //add the record to the database is necessary
            if (!typeExists)
            {
                _db.SubscriptionTypes.Add(record);
            }

            _db.SaveChanges();
        }

        private List<string> GetRecipients(SubscriptionType SubType)
        {
            //get the email addresses of the subscriptions
            string subName = GetTypeName(SubType);
            return _db.Subscriptions
                .Where(s => s.SubscriptionType.Name == subName)
                .Select(s => s.Email == null ? s.User.Email : s.Email)
                .ToList();
        }

        private void CreateAndSendMessages(SubscriptionType SubType, EmailModel EmailModel, bool Async)
        {
            string subName = GetTypeName(SubType);
            List<string> Recipients = GetRecipients(SubType);
            log.DebugFormat("Sending {0} messages to recipients:{1}",
                Recipients.Count,
                String.Join("", Recipients.Select(r => "\n\t- " + r)));

            //create the messages
            //TODO: do on a new thread for async (MVCMailer breaks if you try to do this on a new thread)
            List<MvcMailMessage> Messages;
            try
            {
                Messages = Recipients.Select(r => _mailer.SubscriptionEmail(subName, EmailModel, new MailAddress(r))).ToList();
                log.DebugFormat("Created {0} email successfully", Messages.Count);
            }
            catch (Exception e)
            {
                log.Error("Error creating emails", e);
                throw e;
            }

            //send each message
            try
            {
                foreach (MvcMailMessage Message in Messages)
                {
                    if (Async)
                    {
                        //MVCMailer SendAsync is broken so we have to create a new thread and
                        //copy over the HttpContext to make async sending work
                        //NOTE: this does NOT work on the production server!
                        var currContext = HttpContext.Current;
                        new Task(() =>
                        {
                            //copy the current HttpContext over to the new thread
                            HttpContext.Current = currContext;

                            //send the messages
                            foreach (MvcMailMessage message in Messages)
                            {
                                message.SendAsyncLog(log);
                            }
                        }).Start();
                    }
                    else
                    {
                        Message.SendLog(log);
                    }
                }

                log.DebugFormat("Sent {0} emails successfully", Messages.Count);
            }
            catch (Exception e)
            {
                log.Error(string.Format("Error sending emails"), e);
                throw e;
            }
        }

        #endregion
    }
}