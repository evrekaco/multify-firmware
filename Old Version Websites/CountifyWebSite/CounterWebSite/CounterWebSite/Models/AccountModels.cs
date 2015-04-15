using FoursquareOAuth.ApiClasses;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using System.Diagnostics;
using Microsoft.Web.WebPages.OAuth;
using DatabaseContext;
using Resources;

namespace CounterWebSite.Models
{
    public class LoginModel
    {
        public AuthenticationClientData FSqOAuthProvider { get; set; }
        public string ReturnUrl { get; set; }

        public LoginModel()
        {
            this.FSqOAuthProvider = OAuthWebSecurity.GetOAuthClientData("Foursquare");
        }
    }

    public class RegisterExternalLoginModel
    {
        [Required(ErrorMessageResourceName = "Register_UserName_Required", ErrorMessageResourceType = typeof(AccountStrings))]
        [Display(Name = "Register_UserName", ResourceType = typeof(AccountStrings))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Register_EmailAddress_Required", ErrorMessageResourceType = typeof(AccountStrings))]
        [Display(Name = "Register_EmailAddress", ResourceType = typeof(AccountStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "Register_EmailAddressInvalid", ErrorMessageResourceType = typeof(AccountStrings))]
        public string EmailAddress { get; set; }

        [Display(Name = "Register_PhoneNumber", ResourceType = typeof(AccountStrings))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "Register_PhoneNumberInvalid", ErrorMessageResourceType = typeof(AccountStrings))]
        public string PhoneNumber { get; set; }

        [Display(Name = "Register_ManagedVenues", ResourceType = typeof(AccountStrings))]
        public List<string> ManagedVenues { get; set; }

        [HiddenInput]
        public string ExternalLoginData { get; set; }

        [HiddenInput]
        public string AccessToken { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class ManageAcountModel
    {
        public Uri AccountLink { get; set; }
    }

    public class UpdateAccountModel
    {
        //[Required]
        //[Display(Name = "User name")]
        //public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Register_EmailAddress_Required", ErrorMessageResourceType = typeof(AccountStrings))]
        [Display(Name = "Register_EmailAddress", ResourceType = typeof(AccountStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "Register_EmailAddressInvalid", ErrorMessageResourceType = typeof(AccountStrings))]
        public string EmailAddress { get; set; }

        [Display(Name = "Register_PhoneNumber", ResourceType = typeof(AccountStrings))]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "Register_PhoneNumberInvalid", ErrorMessageResourceType = typeof(AccountStrings))]
        public string PhoneNumber { get; set; }

        public UpdateAccountModel() {}

        public UpdateAccountModel(UserProfile User)
        {
            //this.UserName = User.UserName;
            this.EmailAddress = User.Email;
            this.PhoneNumber = User.PhoneNumber;
        }
    }
}
