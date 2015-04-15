using System.Web;
using System.Web.Http.Filters;

namespace CounterWebSite.Filters
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //log error to Elmah...Web API swallows this by default
            Elmah.ErrorSignal.FromCurrentContext().Raise(context.Exception);
        }
    }
}