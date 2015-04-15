using DatabaseContext;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CounterWebSite.Models
{
    public class NotifySubscriptionModel
    {
        [Required(ErrorMessageResourceName ="NotifySubscription_Email", ErrorMessageResourceType=typeof(SubStrings))]
        [Display(Name = "NotifySubscription_Email", ResourceType = typeof(SubStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName="NotifySubscription_EmailInvalid", ErrorMessageResourceType=typeof(SubStrings))]
        public string Email { get; set; }
    }

    public class SubscriptionDetails
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Type")]
        public string SubscriptionType { get; set; }
        public string SubscriptionTypeDescription { get; set; }

        public bool hasAccount
        {
            get { return UserId.HasValue; }
        }

        public SubscriptionDetails() { }

        public SubscriptionDetails(Subscription subscription)
        {
            this.Id = subscription.Id;
            this.UserId = subscription.UserId;
            this.SubscriptionType = subscription.SubscriptionType.Name;
            this.SubscriptionTypeDescription = subscription.SubscriptionType.Description;

            if(this.hasAccount)
            {
                this.UserName = subscription.User.UserName;
                this.Email = subscription.User.Email;
            }
            else
            {
                this.UserName = null;
                this.Email = subscription.Email;
            }
        }
    }

    public class EditSubscription
    {
        public int TypeId { get; set; }
        public string Description { get; set; }
        public bool Subscribed { get; set; }

        public EditSubscription() { }

        public EditSubscription(DatabaseContext.SubscriptionType SubType)
        {
            Debug.Assert(SubType.AdministratorOnly ? Roles.IsUserInRole("Administrators") : true);
            TypeId = SubType.Id;
            Subscribed = SubType.Subscriptions.Any(s => s.User != null && s.UserId == WebSecurity.CurrentUserId);
            Description = SubType.Description;
        }
    }
}