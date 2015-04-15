using DatabaseContext;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Filters
{
    public class RequestLogFilter : ActionFilterAttribute
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestLogFilter));

        public List<string> ExcludedControllers { get; set; }
        public List<string> ExcludedActions { get; set; }

        public RequestLogFilter(List<string> ExcludedControllers = null, List<string> ExcludedActions = null)
            : base()
        {
            if(ExcludedControllers != null)
            {
                this.ExcludedControllers = ExcludedControllers;
            }
            else
            {
                this.ExcludedControllers = new List<string>();
            }

            if (ExcludedActions != null)
            {
                this.ExcludedActions = ExcludedActions;
            }
            else
            {
                this.ExcludedActions = new List<string>();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            HttpRequestBase request = filterContext.HttpContext.Request;
            RequestRecord requestInfo;

            try
            {
                //extract request data
                var controller = filterContext.Controller;
                var routeData = request.RequestContext.RouteData;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < request.Headers.Count; i++)
                    sb.AppendFormat("{0}={1};", request.Headers.Keys[i],
                                                request.Headers[i].ToString());

                //return if the current controller or action has been exluded from logging
                if(this.ExcludedControllers.Contains((string)routeData.Values["controller"]) ||
                    this.ExcludedActions.Contains((string)routeData.Values["action"]))
                {
                    return;
                }

                //create a log item
                requestInfo = new RequestRecord()
                {
                    RequestDate = DateTime.Now,
                    RequestType = request.RequestType,
                    Url = request.RawUrl,
                    IPAddress = request.UserHostAddress,
                    Controller = (string)routeData.Values["controller"],
                    Action = (string)routeData.Values["action"],
                    RequestHeader = sb.ToString()
                };

                //read the request content
                using (StreamReader reader = new StreamReader(request.InputStream))
                {
                    try
                    {
                        request.InputStream.Position = 0;
                        requestInfo.RequestBody = reader.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        requestInfo.RequestBody = "ERROR_READING_REQUEST_BODY:" + ex.Message;
                    }
                    finally
                    {
                        request.InputStream.Position = 0;
                    }
                }

                //log the request
                log.Info(String.Format("{0} request made by {1} to url: {2}", requestInfo.RequestType, requestInfo.IPAddress, requestInfo.Url));
                log.Info(String.Format("Request body:{0}", requestInfo.RequestBody));
                
                //save request info to database
                using (CounterDbContext db = new CounterDbContext())
                {
                    db.RequestLog.Add(requestInfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //error will be logged by elmah
                throw ex;
            }
        }
    }
}