using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactoriesManager.Api.Results
{
    public class AcceptedNegotiatedContentResult<T> : IHttpActionResult
    {
        private readonly ApiController _apiController;
        private readonly T _content;
        private readonly string _routeName;
        private readonly IDictionary<string, object> _routeValues;

        public AcceptedNegotiatedContentResult(ApiController apiController, T content)
        {
            _apiController = apiController;
            _content = content;
        }

        public AcceptedNegotiatedContentResult(ApiController apiController, string routeName, IDictionary<string, object> routeValues)
        {
            _apiController = apiController;
            _routeName = routeName;
            _routeValues = routeValues;
        }

        public virtual Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var request = _apiController.Request;
            var formatters = _apiController.Configuration.Formatters;
            var contentNegotiator = _apiController.Configuration.Services.GetContentNegotiator();
            var urlFactory = _apiController.Url;
            var negotiationResult = contentNegotiator.Negotiate(typeof(T), request, formatters);
            var httpResponseMessage = new HttpResponseMessage();
            try
            {
                if (negotiationResult == null)
                {
                    httpResponseMessage.StatusCode = HttpStatusCode.NotAcceptable;
                }
                else
                {
                    httpResponseMessage.StatusCode = HttpStatusCode.Accepted;
                    httpResponseMessage.StatusCode = HttpStatusCode.Created;
                    if (_routeName != null)
                    {
                        string uriString = urlFactory.Link(this._routeName, this._routeValues);
                        httpResponseMessage.Headers.Location = new Uri(uriString);
                    }
                    if (_content != null)
                    {
                        httpResponseMessage.Content = new ObjectContent<T>(_content, negotiationResult.Formatter, negotiationResult.MediaType);
                    }
                }
                httpResponseMessage.RequestMessage = _apiController.Request;
            }
            catch
            {
                httpResponseMessage.Dispose();
                throw;
            }
            return Task.FromResult(httpResponseMessage);
        }
    }
}