using System.Net;

namespace FactoriesManager.Api.Helpers
{
    internal static class ExtendedStatusCodes
    {
        public const HttpStatusCode UnprocessableEntity = (HttpStatusCode) 422;
    }
}