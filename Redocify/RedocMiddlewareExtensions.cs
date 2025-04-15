using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Asp.Versioning.ApiExplorer;
using Redocify.Endpoints;

namespace Redocify
{
    public static class RedocMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedocify(
            this IApplicationBuilder app,
            IServiceProvider serviceProvider,
            string route = "/redoc",
            string swaggerUrl = "/swagger/v1/swagger.json")
        {
            var assembly = Assembly.GetExecutingAssembly();

            var apiProvider = serviceProvider.GetService<IApiVersionDescriptionProvider>();

            app.UseSwagger();

            if (apiProvider != null)
            {
                app.UseSwaggerUI(options =>
                {
                    foreach (var desc in apiProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                    }
                });

                app.MapRedocifyEndpoints(apiProvider, route, swaggerUrl);
            }
            else
            {
                // اگر نسخه‌بندی نبود، یه endpoint ساده فقط برای Redoc بساز
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(swaggerUrl, "API");
                });

                app.MapRedocifyEndpoints(null, route, swaggerUrl);
            }

            app.MapRedocifyEndpoints(apiProvider, route, swaggerUrl);

            return app;
        }
    }
}
