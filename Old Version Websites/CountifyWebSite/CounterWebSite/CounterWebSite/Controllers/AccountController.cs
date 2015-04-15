using CounterWebSite.Filters;
using CounterWebSite.Helpers;
using CounterWebSite.Models;
using DatabaseContext;
using DotNetOpenAuth.AspNet;
using FoursquareOAuth;
using FoursquareOAuth.ApiClasses;
using log4net;
using Microsoft.Web.WebPages.OAuth;
using Newtonsoft.Json.Linq;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CounterWebSite.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private static readonly string adminRoleName = "Administrators";

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View(new LoginModel()
                {
                    ReturnUrl = returnUrl
                });
        }

        [AllowAnonymous]
        public ActionResult IfPer()
        {
            return View(new LoginModel()
            {
                
            });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage()
        {
            OAuthAccount FsqAccount = OAuthWebSecurity.GetAccountsFromUserName(WebSecurity.CurrentUserName).First();
            ManageAcountModel Model = new ManageAcountModel();

            Model.AccountLink = _FSqApi.GetProfileUrl(FsqAccount.ProviderUserId);

            return View(Model);
        }

        //
        // GET: /Account/Update

        public ActionResult Update()
        {
            return PartialView(new UpdateAccountModel(GetLoggedInUserProfile()));
        }

        //
        // POST: /Account/Update

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UpdateAccountModel Model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(Model);
            }

            // Make sure this user name is unique
            //if (db.UserProfiles.Any(u => u.UserId != WebSecurity.CurrentUserId && u.UserName.ToLower() == Model.UserName.ToLower()))
            //{
            //    ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
            //    return PartialView(Model);
            //}

            //make sure the email address is unique
            if (db.UserProfiles.Any(u => u.UserId != WebSecurity.CurrentUserId && u.Email.ToLower() == Model.EmailAddress.ToLower()))
            {
                ModelState.AddModelError("EmailAddress", AccountStrings.update_emailExists);
                return PartialView(Model);
            }

            //update the account information
            UserProfile user = GetLoggedInUserProfile();
            //user.UserName = Model.UserName;       changing this breaks simplemembership authentication (unless force a logoff on the user and then they must log in again)s
            user.Email = Model.EmailAddress;
            user.PhoneNumber = Model.PhoneNumber;
            db.SaveChanges();

            SetAjaxStatusMessage(AccountStrings.update_successMessage, StatusType.Success);
            return PartialView(Model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            //if the user logged in as a venue, redirect them to a page to fix the mistake
            if (result.ExtraData["type"] == "venuePage")
            {
                return RedirectToAction("ExternalLoginMistake");
            }

            //TODO: don't let user connect to more than one client?

            //check if this account is already associated by attempting to log in
            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                //refesh this user's managed venues, then return to the previous page
                UpdateManagedVenues(db.UserProfiles.First(u => u.FoursquareId == result.ProviderUserId));
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // A user is already logged in; associate the new oauth account
                //NOTE: this should never happen since only have a single authentication source for this site
                //throw new Exception("Authenticated user tried to log in again");
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }

            //get the venues managed by this user
            List<FSqVenue> venues = _FSqApi.GetManagedVenues(result.ExtraData["oauth_token"]);
            //if (venues == null || venues.Count == 0)
            //{
            //    SetStatusMessage(AccountStrings.login_notManager);
            //    return RedirectToAction("ExternalLoginMistake", "Account");
            //}

            // User is new, ask for their desired membership info
            string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;

            var model = new RegisterExternalLoginModel
            {
                UserName = result.UserName,
                AccessToken = result.ExtraData["oauth_token"],
                EmailAddress = result.ExtraData["email"],
                PhoneNumber = result.ExtraData["phone"],
                ManagedVenues = venues.Select(v => v.name).ToList(),
                ExternalLoginData = loginData
            };
            return View("ExternalLoginConfirmation", model);
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            //set viewbag variables in case we have to redisplay the form
            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;

            //make sure the model state is valid
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Make sure this user name is unique
            if (db.UserProfiles.Any(u => u.UserName.ToLower() == model.UserName.ToLower()))
            {
                ModelState.AddModelError("UserName", AccountStrings.login_usernameExists);
                return View(model);
            }

            //make sure the email address is unique
            if (db.UserProfiles.Any(u => u.Email.ToLower() == model.EmailAddress.ToLower()))
            {
                ModelState.AddModelError("EmailAddress", AccountStrings.login_emailExists);
                return View(model);
            }

            //make sure the access token has been saved
            if (db.AccessTokens.Find(providerUserId) == null)
            {
                log.Error("No access token was saved during authentication");
                throw new Exception("No access token was saved during authentication");
            }

            // Create a new user profile
            UserProfile newUser = new UserProfile
            {
                UserName = model.UserName,
                AccessToken = model.AccessToken,
                Email = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                ManagedVenues = new List<Venue>(),
                FoursquareId = providerUserId
            };
            db.UserProfiles.Add(newUser);
            log.Info(String.Format("Created new user '{0}'", newUser.UserName));

            //get the venues managed by this user
            List<FSqVenue> managedVenues = _FSqApi.GetManagedVenues(newUser.AccessToken);
            //if (managedVenues == null || managedVenues.Count == 0)
            //{
            //    log.Warn("Failed to register user, since they mange no venues");
            //    SetStatusMessage(AccountStrings.login_notManager);
            //    return RedirectToAction("ExternalLoginMistake", "Account");
            //}

            //create manager associations for this user
            foreach (FSqVenue FSqVenue in managedVenues)
            {
                Venue dbVenue = db.Venues.Find(FSqVenue.id);

                //make sure the venue exists in the db
                if (dbVenue == null)
                {
                    dbVenue = new Venue()
                    {
                        VenueId = FSqVenue.id,
                        Name = FSqVenue.name,
                        CheckinCount = FSqVenue.stats.checkinsCount,
                        lastUpdated = DateTime.Now
                    };
                    db.Venues.Add(dbVenue);
                    log.Info(String.Format("Created new venue '{0}'", dbVenue.Name));
                }

                //add this user as a manager for the venue
                newUser.ManagedVenues.Add(dbVenue);
                log.Info(String.Format("Added '{0}' as a manager for '{1}'", newUser.UserName, dbVenue.Name));
            }

            db.SaveChanges();

            //create the account and login
            OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
            OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);
            log.Info(String.Format("OAuth account created for {0}", model.UserName));

            //associate any existing subscriptions
            //TODO: allow user to select registration on the form
            //TODO: make this work properly
            SubscriptionManager _sm = new SubscriptionManager(db);
            _sm.MigrateSubscriptions(newUser.UserName);

            //if the user manages the innovaction venue, make them an administrator
            string InnovactionVenueName = getInnovactionName(newUser.AccessToken);
            if (managedVenues.Select(v => v.name).Contains(InnovactionVenueName))
            {
                Roles.AddUserToRole(newUser.UserName, adminRoleName);
                log.Info(String.Format("Added Innovaction manager {0} to the 'administrator' role.", newUser.UserName));
            }

            SetStatusMessage(AccountStrings.login_createSuccess);
            return RedirectToLocal(returnUrl);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //
        // GET: /Account/ExternalLoginMistake

        [AllowAnonymous]
        public ActionResult ExternalLoginMistake()
        {
            return View();
        }

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //}

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        public ActionResult ConfirmDelete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            //get the currently logged in user
            string userName = WebSecurity.CurrentUserName;
            int userId = WebSecurity.CurrentUserId;
            SimpleMembershipProvider simpleMembership = (SimpleMembershipProvider)Membership.Provider;

            UserProfile user = db.UserProfiles.Find(userId);

            //delete subscriptions
            foreach (Subscription subscription in db.Subscriptions.Where(s => s.UserId == userId))
            {
                db.Subscriptions.Remove(subscription);
            }
            db.SaveChanges();

            //remove user from any roles
            if (Roles.GetRolesForUser(userName).Count() > 0)
            {
                Roles.RemoveUserFromRoles(userName, Roles.GetRolesForUser(userName));
            }

            //delete local account (deletes record from webpages_Membership table)
            if (simpleMembership.HasLocalAccount(userId))
            {
                simpleMembership.DeleteAccount(userName);
            }

            //delte OAuth account(s)
            foreach (OAuthAccount account in OAuthWebSecurity.GetAccountsFromUserName(userName))
            {
                OAuthWebSecurity.DeleteAccount(account.Provider, account.ProviderUserId);
            }

            //delete user (deletes record from userProfile table)
            //TODO: optionally delete all data
            simpleMembership.DeleteUser(userName, true);

            //logout and redirect to home page
            WebSecurity.Logout();
            SetStatusMessage(AccountStrings.delete_success);
            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private string getInnovactionName(string accessToken)
        {
            const string InnovactionVenueId = "52f8a295498e04874f8d15ca";
            JObject InnovactionVenue = _FSqApi.queryApi(accessToken, "venues/" + InnovactionVenueId);
            return InnovactionVenue["venue"]["name"].ToString();
        }

        private bool IsInnovactionManager(string accessToken)
        {
            //get the innovaction Foursquare name
            string InnovactionVenueName = getInnovactionName(accessToken);

            //get the venues managed by this user
            List<FSqVenue> venues = _FSqApi.GetManagedVenues(accessToken);
            if (venues != null && venues.Count > 0 &&
                venues.Select(v => v.name).Contains(InnovactionVenueName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the venues that the specified user manages and update the database to reflect any changes
        /// </summary>
        /// <param name="AccessToken">The Foursquare access token for the user</param>
        private void UpdateManagedVenues(UserProfile User)
        {
            log.DebugFormat("Updating managed venues for user {0}", User.UserName);

            //get the managed venues for this user
            FSqApiInterface _FSqApi = new FSqApiInterface(new AccessTokenManager());
            List<FSqVenue> managedVenues = _FSqApi.GetManagedVenues(User.AccessTokenRef.Token);
            log.DebugFormat("Retrieved manager list from fourquare: {0}", string.Join(",", managedVenues.Select(v => v.name).ToArray()));

            //create any new venues that don't exist in the database
            foreach (FSqVenue newVenue in managedVenues.Where(v => db.Venues.Find(v.id) == null))
            {
                db.Venues.Add(new Venue()
                {
                    VenueId = newVenue.id,
                    Name = newVenue.name,
                    //TODO: send request to get checkins
                    //Checkins = {not available in compact venue}
                    lastUpdated = DateTime.Now,
                    Managers = new List<UserProfile>()
                });
                log.InfoFormat("Saved new vennue to database: {0}", newVenue.name);
            }
            db.SaveChanges();

            //create the managed venue list if needed
            if (User.ManagedVenues == null)
            {
                User.ManagedVenues = new List<Venue>();
            }

            var existsingVenueIds = User.ManagedVenues.Select(v => v.VenueId).ToList();
            var newVenueIds = managedVenues.Select(v => v.id).ToList();

            //add new manager venues
            foreach (string venueId in newVenueIds.Where(id => !existsingVenueIds.Contains(id)))
            {
                User.ManagedVenues.Add(db.Venues.Find(venueId));
                log.InfoFormat("Added user {0} as manager for venue {1}", User.UserName, venueId);
            }

            //remove old manager venues
            foreach (string venueId in existsingVenueIds.Where(id => !newVenueIds.Contains(id)))
            {
                User.ManagedVenues.Remove(db.Venues.Find(venueId));
                log.InfoFormat("Removed user {0} as manager for venue {1}", User.UserName, venueId);
            }

            db.SaveChanges();
            log.DebugFormat("Finished updating managed venues for user {0}: {1}", User.UserName, String.Join(",", User.ManagedVenues.Select(v => v.Name).ToArray()));

            //check if this user is a manager
            bool isAdmin = User.ManagedVenues.Select(v => v.Name).ToList().Contains(getInnovactionName(User.AccessTokenRef.Token));
            bool isInAdminRole = Roles.IsUserInRole(User.UserName, adminRoleName);
            if (isAdmin && !isInAdminRole)
            {
                Roles.AddUserToRole(User.UserName, adminRoleName);
            }
            else if(!isAdmin && isInAdminRole)
            {
                Roles.RemoveUserFromRole(User.UserName, adminRoleName);
            }
        }

        #endregion

        #region RemovedFormsLoginContent

        ////
        //// POST: /Account/Login

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    ModelState.AddModelError("", "The user name or password provided is incorrect.");
        //    return View(model);
        //}

        ////
        //// GET: /Account/Register

        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/Register

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Attempt to register the user
        //        try
        //        {
        //            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
        //            WebSecurity.Login(model.UserName, model.Password);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        catch (MembershipCreateUserException e)
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        //// POST: /Account/Manage

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public ActionResult Manage(LocalPasswordModel model)
        //public ActionResult Manage()
        //{
        //    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    ViewBag.HasLocalPassword = hasLocalAccount;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasLocalAccount)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // ChangePassword will throw an exception rather than return false in certain failure scenarios.
        //            bool changePasswordSucceeded;
        //            try
        //            {
        //                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        //            }
        //            catch (Exception)
        //            {
        //                changePasswordSucceeded = false;
        //            }

        //            if (changePasswordSucceeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User does not have a local password so remove any validation errors caused by a missing
        //        // OldPassword field
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            catch (Exception)
        //            {
        //                ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
        //            }
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //{
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus)
        //    {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "User name already exists. Please enter a different user name.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "The password provided is invalid. Please enter a valid password value.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "The e-mail address provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "The password retrieval question provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "The user name provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        default:
        //            return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //    }
        //}

        #endregion
    }
}
