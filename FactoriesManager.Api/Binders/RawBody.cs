using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace FactoriesManager.Api.Binders
{
    /// <summary>
    /// An attribute that captures the entire content body and stores it
    /// into the parameter of type string or byte[].
    /// </summary>
    /// <remarks>
    /// The parameter marked up with this attribute should be the only parameter as it reads the
    /// entire request body and assigns it to that parameter.    
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    internal sealed class FromRawBodyAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
                throw new ArgumentException("Invalid parameter");

            return new RawBodyParameterBinding(parameter);
        }

        /// <summary>
        /// Reads the Request body into a string/byte[] and
        /// assigns it to the parameter bound.
        /// </summary>
        private class RawBodyParameterBinding : HttpParameterBinding
        {
            public RawBodyParameterBinding(HttpParameterDescriptor descriptor)
                : base(descriptor)
            {
            }

            public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
                HttpActionContext actionContext,
                CancellationToken cancellationToken)
            {
                if (actionContext.Request.Method == HttpMethod.Get)
                {
                    return Task.Run(() => { }, cancellationToken);
                }

                var type = Descriptor.ParameterType;

                if (type == typeof(string))
                {
                    return actionContext.Request.Content
                        .ReadAsStringAsync()
                        .ContinueWith(task =>
                        {
                            var stringResult = task.Result;
                            SetValue(actionContext, stringResult);
                        }, cancellationToken);
                }
                if (type == typeof(byte[]))
                {
                    return actionContext.Request.Content
                        .ReadAsByteArrayAsync()
                        .ContinueWith(task =>
                        {
                            var result = task.Result;
                            SetValue(actionContext, result);
                        }, cancellationToken);
                }

                throw new InvalidOperationException("Only string and byte[] are supported for [FromRawBody] parameters");
            }

            public override bool WillReadBody => true;
        }
    }
}