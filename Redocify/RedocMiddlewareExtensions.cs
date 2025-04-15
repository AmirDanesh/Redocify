using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace Redocify
{
    public static class RedocMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedocify(
            this IApplicationBuilder app,
            string route = "/redoc",
            string swaggerUrl = "/swagger/v1/swagger.json")
        {
            var assembly = Assembly.GetExecutingAssembly();

            app.Map($"{route}/custom.css", builder =>
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

            app.Map($"{route}/redoc.standalone.min.js", builder =>
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

            app.Map(route, builder =>
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

            return app;
        }
    }
}
