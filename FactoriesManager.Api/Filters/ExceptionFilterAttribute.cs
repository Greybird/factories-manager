using System;
using System.Net;
using System.Net.Http;
using FactoriesManager.Api.Exceptions;
using FactoriesManager.Api.Helpers;
using FactoriesManager.Exceptions;

namespace FactoriesManager.Api.Filters
{
    internal class ExceptionFilterAttribute : ExceptionFilterBaseAttribute
    {
        protected override HttpResponseMessage HandleSpecificException(Exception exception)
        {
            var closedReport = exception as ClosedReportException;
            if (closedReport != null)
            {
                return new HttpResponseMessage(ExtendedStatusCodes.UnprocessableEntity) {Content = new StringContent(closedReport.Message)};
            }
            var resultsNotFound = exception as ResultsNotFoundException;
            if (resultsNotFound != null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound) {Content = new StringContent(resultsNotFound.Message)};
            }
            var outdated = exception as OutdatedReportIdException;
            if (outdated != null)
            {
                return new HttpResponseMessage(ExtendedStatusCodes.UnprocessableEntity) {Content = new StringContent(outdated.Message)};
            }
            var reportNotFound = exception as ReportNotFoundException;
            if (reportNotFound != null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound) {Content = new StringContent(reportNotFound.Message)};
            }
            var jobNotFound = exception as JobNotFoundException;
            if (jobNotFound != null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound) {Content = new StringContent(jobNotFound.Message)};
            }
            return null;
        }
    }
}