using CounterWebSite.Controllers;
using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Helpers
{
    public static class CustomHtmlHelpers
    {
        public static IHtmlString StatusMessage(this HtmlHelper Html, string Classes = "")
        {
            return CreateStatusMessage(Html, Classes, false);
        }

        public static IHtmlString AjaxStatusMessage(this HtmlHelper Html, string Classes = "")
        {
            return CreateStatusMessage(Html, Classes, true);
        }

        private static IHtmlString CreateStatusMessage(this HtmlHelper Html, string Classes, bool isAjax)
        {
            var TempData = Html.ViewContext.TempData;

            //make sure a status message was set
            if (!TempData.ContainsKey(CounterWebSite.Controllers.BaseController.TempDataStatusKey))
            {
                return new HtmlString("");
            }

            //make sure this is the correct type of status message
            if ((bool)TempData[CounterWebSite.Controllers.BaseController.TempDataStatusIsAjaxKey] != isAjax)
            {
                return new HtmlString("");
            }

            return Html.Raw(String.Format(@"<div class=""status-message {0} {1}""><p class=""{2}"">{3}</p></div>",
                isAjax ? "ajax-status" : "",
                Classes,
                TempData[CounterWebSite.Controllers.BaseController.TempDataStatusTypeKey],
                TempData[CounterWebSite.Controllers.BaseController.TempDataStatusKey]));
        }
    }
}