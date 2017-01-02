using System;
using System.IO;
using System.Web.Http;
using FactoriesManager.Api;
using FactoriesManager.Api.Filters;
using Hangfire;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Converters;
using Owin;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(Startup))]

namespace FactoriesManager.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new ExceptionFilterAttribute());

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter {AllowIntegerValues = false});

            GlobalConfiguration.Configuration.UseSqlServerStorage("Hangfire");
            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer();

            config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Factories Manager API");
                    GetXmlCommentsPathForModels(c);
                    c.DescribeAllEnumsAsStrings();
                })
                .EnableSwaggerUi();

            app.UseWebApi(config);
        }

        private static void GetXmlCommentsPathForModels(SwaggerDocsConfig config)
        {
            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var commentFile in baseDirectory.GetFiles("FactoriesManager.*.xml"))
            {
                config.IncludeXmlComments(commentFile.FullName);
            }
        }

        public static IDisposable Run(string baseAddress)
        {
            // Start OWIN host 
            return WebApp.Start<Startup>(url: baseAddress);
        }
    }
}