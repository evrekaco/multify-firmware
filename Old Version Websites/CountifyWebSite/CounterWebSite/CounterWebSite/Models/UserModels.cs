using DatabaseContext;
using FoursquareOAuth.ApiClasses;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Models
{
    public class UserEdit
    {
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public UserEdit() { }

        public UserEdit(UserProfile user)
        {
            this.UserId = user.UserId;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
        }
    }

    public class UserDetails
    {
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foursquare Account")]
        [DataType(DataType.Url)]
        public string ExternalAccount { get; set; }

        public List<string> Roles { get; set; }

        [Display(Name = "Managed Venues")]
        public List<VenueDetails> ManagedVenues { get; set; }

        public List<SubscriptionDetails> Subscriptions { get; set; }

        public UserDetails() { }

        public UserDetails(UserProfile user)
        {
            this.UserId = user.UserId;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.Roles = System.Web.Security.Roles.GetRolesForUser(user.UserName).ToList();
            this.ManagedVenues = user.ManagedVenues.Select(v => new VenueDetails()
                {
                    VenueId = v.VenueId,
                    Name = v.Name
                }).ToList();
            this.Subscriptions = user.Subscriptions.Select(s => new SubscriptionDetails(s)).ToList();

            OAuthAccount FSqAccount = OAuthWebSecurity.GetAccountsFromUserName(user.UserName)
                .FirstOrDefault(a => a.Provider == "Foursquare");
            if (FSqAccount != null)
            {
                //TODO: retrieve from foursquareapi
                this.ExternalAccount = "https://foursquare.com/user/" + FSqAccount.ProviderUserId;
            }
        }
    }
}