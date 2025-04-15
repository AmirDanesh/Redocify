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
            app.Map(route, builder =>
            {
                builder.Run(async context =>
                {
                    var assembly = Assembly.GetExecutingAssembly();

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
