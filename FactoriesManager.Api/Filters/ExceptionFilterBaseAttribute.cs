using System;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Filters;

namespace FactoriesManager.Api.Filters
{
    internal abstract class ExceptionFilterBaseAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var response = HandleException(context.Exception);
            if (response != null)
            {
                context.Response = response;
            }
        }

        private HttpResponseMessage HandleException(Exception exception)
        {
            var targetInvocationException = exception as TargetInvocationException;
            if (targetInvocationException != null)
            {
                return HandleException(targetInvocationException.InnerException);
            }
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                return HandleException(aggregateException.InnerException);
            }
            return HandleSpecificException(exception);
        }

        protected abstract HttpResponseMessage HandleSpecificException(Exception exception);
    }
}