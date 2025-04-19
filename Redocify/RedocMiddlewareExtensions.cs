using System;
using System.Linq;
using System.Reflection;
using Redocify.Endpoints;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redocify.Configurations;

namespace Redocify
{
    public static class RedocMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedocify(
            this IApplicationBuilder app,
            Action<RedocifyConfigs> options = null)
        {
            var configuration = new RedocifyConfigs();
            if (options != null) options?.Invoke(configuration);

            app.UseStaticFiles();
            app.UseSwagger();
            var assembly = Assembly.GetExecutingAssembly();
            var groupNames = GetGroupNames(app.ApplicationServices);

            if (groupNames.Any())
            {
                app.UseSwaggerUI(options =>
                {
                    foreach (var desc in groupNames)
                    {
                        options.SwaggerEndpoint($"/swagger/{desc}/swagger.json", desc.ToUpperInvariant());
                    }
                });
            }
            else
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(configuration.SwaggerUrl, "API");
                });
            }

            app.MapRedocifyEndpoints(groupNames, configuration.LaunchRoute, configuration.SwaggerUrl);

            return app;
        }

        private static List<string> GetGroupNames(IServiceProvider serviceProvider)
        {
            var apiProvider = serviceProvider.GetService<Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider>();

            List<string> result = new();

            if (apiProvider is not null)
                result = apiProvider.ApiVersionDescriptions.Select(x => x.GroupName).ToList();

            return result;
        }
    }
}
