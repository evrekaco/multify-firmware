using CounterWebSite.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HomeController() : base(false) { }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Set the culture then go to the specified page
        /// </summary>
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);

            // Get or create the cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie == null)
            {
                cookie = new HttpCookie("_culture");
                cookie.Expires = DateTime.Now.AddYears(1);
            }

            //set the cookie value
            cookie.Value = culture;
            Response.Cookies.Add(cookie);

            //Return to the specified page
            return RedirectToLocal(returnUrl);
        }
    }
}
