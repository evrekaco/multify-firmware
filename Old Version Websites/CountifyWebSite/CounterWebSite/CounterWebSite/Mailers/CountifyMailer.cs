using CounterWebSite.Models;
using log4net;
using Mvc.Mailer;
using PreMailer.Net;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Mvc;

namespace CounterWebSite.Mailers
{
    public class CountifyMailer : MailerBase, ICountifyMailer
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(CountifyMailer));
        //private UrlHelper Url = new Url

        public CountifyMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage SubscriptionEmail(String ViewName, EmailModel model, MailAddress recipient)
        {
            //set the View Model
            ViewData.Model = model;

            MvcMailMessage message = Populate(x =>
            {
                x.Subject = model.Subject;
                x.ViewName = ViewName;
                x.To.Add(recipient);
                x.From = new MailAddress("admin@countify.co", "Countify Admin");
                //x.LinkedResources = new Dictionary<string, string>()
                //    {
                //        { "logo", "Images/countify_logo.png" }
                //    };
            });

            //move styles inline
            var inlineResult = PreMailer.Net.PreMailer.MoveCssInline(message.Body);
            message.Body = inlineResult.Html;

            //log warnings from inlining
            foreach(string warning in inlineResult.Warnings)
            {
                log.Warn("Warning while inlining css styles: " + warning);
            }

            return message;
        }
    }

    /// <summary>
    /// Extension methods for the MvcMailMessage class
    /// </summary>
    public static class MailExtensions
    {
        private const int SmtpTimeout = 10000;

        /// <summary>
        /// Extension method to log the result of an async email send
        /// </summary>
        public static void SendAsyncLog(this MvcMailMessage Message, ILog log)
        {
            string recipients = String.Join(";", Message.To.Select(r => r.ToString()).ToArray());
            log.InfoFormat("Sending email to '{0}'", recipients);

            //check for success on completion
            SmtpClientWrapper smtp = new SmtpClientWrapper();
            log.DebugFormat("Using smtp settings: {0}:{1}", smtp.InnerSmtpClient.Host, smtp.InnerSmtpClient.Port);
            smtp.InnerSmtpClient.Timeout = SmtpTimeout;
            smtp.SendCompleted += (sender, e) =>
            {
                //string recipients = (string)e.UserState;

                if (e.Error != null)
                {
                    log.Error(string.Format("Failed to send email to {0}", recipients), e.Error);
                    Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(e.Error));
                    //swallow exception on background thread
                    //TODO: pass this error back in a callback method
                    //throw e.Error;
                }
                else if (e.Cancelled)
                {
                    log.WarnFormat("Sending email to {0} was cancelled", recipients);
                }
                else
                {
                    log.InfoFormat("Sent email to '{0}' successfully", recipients);
                }
            };

            Message.SendAsync(userState: recipients, smtpClient: smtp);
        }

        public static void SendLog(this MvcMailMessage Message, ILog log)
        {
            int NumAttempts = 3;
            string recipients = String.Join(";", Message.To.Select(r => r.ToString()).ToArray());
            log.InfoFormat("Sending email to '{0}'", recipients);

            for (int attempt = 1; attempt <= NumAttempts; attempt++)
            {
                log.InfoFormat("Sending email to '{0}' (attempt {1})", recipients, attempt);
                SmtpClientWrapper smtp = new SmtpClientWrapper();
                smtp.InnerSmtpClient.Timeout = SmtpTimeout;

                try
                {
                    Message.Send(smtp);
                }
                catch (Exception e)
                {
                    log.Error(string.Format("Failed to send email to {0}", recipients), e);
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                    
                    //throw error on the last attempt
                    if (attempt == NumAttempts)
                    {
                        throw e;
                    }
                }
            }
            
            log.InfoFormat("Sent email to '{0}' successfully", recipients);
        }
    }
}