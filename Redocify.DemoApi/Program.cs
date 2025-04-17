using Redocify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRedocify(opt =>
{
    opt.LaunchRoute = "/redoc"; //optional
    opt.SwaggerUrl = "/swagger/v1/swagger.json"; //optional
});

app.UseAuthorization();
app.MapControllers();
app.Run();
