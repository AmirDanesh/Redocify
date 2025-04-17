using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Redocify.Endpoints
{
    public static class RedocifyEndpointExtensions
    {
        public static void MapRedocifyEndpoints(
            this IApplicationBuilder app,
            List<string> groupNames,
            string defaultRoute,
            string swaggerUrl)
        {
            var assembly = Assembly.GetExecutingAssembly();
            app.Map($"{defaultRoute}/apiVersions", builder =>
            {
                builder.Run(async context =>
                {
                    if (groupNames.Any())
                    {
                        var versions = groupNames.Select(x => new
                        {
                            name = x,
                            url = $"/swagger/{x}/swagger.json"
                        });

                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(versions);
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(Array.Empty<object>());
                    }
                });
            });

            app.Map($"{defaultRoute}/custom.css", builder =>
            {
                builder.Run(async context =>
                {
                    var cssStream = assembly.GetManifestResourceStream("Redocify.Resources.custom.css");
                    if (cssStream == null)
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }

                    context.Response.ContentType = "text/css";
                    await cssStream.CopyToAsync(context.Response.Body);
                });
            });

            app.Map($"{defaultRoute}/redoc.standalone.min.js", builder =>
            {
                builder.Run(async context =>
                {
                    var jsStream = assembly.GetManifestResourceStream("Redocify.Resources.redoc.standalone.min.js");
                    if (jsStream == null)
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }

                    context.Response.ContentType = "application/javascript";
                    await jsStream.CopyToAsync(context.Response.Body);
                });
            });

            app.Map(defaultRoute, builder =>
            {
                builder.Run(async context =>
                {
                    var htmlStream = assembly.GetManifestResourceStream("Redocify.Resources.Index.html");
                    var reader = new StreamReader(htmlStream!);
                    var html = await reader.ReadToEndAsync();
                    html = html.Replace("{{SWAGGER_JSON_URL}}", swaggerUrl);

                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(html);
                });
            });
        }
    }
}
