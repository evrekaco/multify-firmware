using CounterWebSite.Filters;
using CounterWebSite.Helpers;
using DatabaseContext;
using FoursquareOAuth;
using log4net;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace CounterWebSite.Controllers
{
    [InitializeSimpleMembership]
    public class BaseController : Controller, IDisposable
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(BaseController));
        protected readonly FSqApiInterface _FSqApi = new FSqApiInterface(new AccessTokenManager());
        protected CounterDbContext db;

        public static readonly string TempDataStatusKey = "statusMessage";
        public static readonly string TempDataStatusTypeKey = "statusType";
        public static readonly string TempDataStatusIsAjaxKey = "statusIsAjax";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="createDbContext">
        /// Can be set to false if the controller doesn't need to use the database, or if it
        /// needs more precice control over the context lifetime for performance reasons
        /// </param>
        public BaseController(bool createDbContext = true)
        {
            if(createDbContext)
            {
                db = new CounterDbContext();
            }
        }


        /// <summary>
        /// Set the thread culture using a cookie or HTTP header then return base method
        /// </summary>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        protected enum StatusType { Success, Info, Warning, Error };

        /// <summary>
        /// Set a status message to be displayed by the layout view
        /// </summary>
        /// <param name="statusMessage"></param>
        protected void SetStatusMessage(string statusMessage, StatusType statusType = StatusType.Info)
        {
            SetStatusMessage(statusMessage, statusType, false);
        }

        /// <summary>
        /// Set a status message to be displayed by the a view retuned via AJAX
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="statusType"></param>
        protected void SetAjaxStatusMessage(string statusMessage, StatusType statusType = StatusType.Info)
        {
            SetStatusMessage(statusMessage, statusType, true);
        }

        private void SetStatusMessage(string statusMessage, StatusType statusType, bool IsAjax)
        {
            if (!string.IsNullOrEmpty(statusMessage))
            {
                TempData[TempDataStatusIsAjaxKey] = IsAjax;
                TempData[TempDataStatusKey] = statusMessage;
                TempData[TempDataStatusTypeKey] = System.Enum.GetName(typeof(StatusType), statusType);
                log.Debug("Set status message:" + statusMessage);
            }
        }

        /// <summary>
        /// Gets the user profile for the current user.
        /// Should only be called when it is certain a user is logged in
        /// </summary>
        /// <returns></returns>
        protected UserProfile GetLoggedInUserProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                string errorMessage = "Error retrieving user profile: no user is logged in.";
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            else
            {
                return db.UserProfiles.Find(WebSecurity.CurrentUserId);
            }
        }

        void IDisposable.Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }

            base.Dispose();
        }
    }
}