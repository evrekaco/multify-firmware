using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
#if DEBUG
            filters.Add(new Filters.RequestLogFilter(
                ExcludedControllers: new List<String>() { "Elmah" },
                ExcludedActions: null));
#endif
        }
    }
}