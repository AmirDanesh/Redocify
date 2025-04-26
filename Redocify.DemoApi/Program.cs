using Redocify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRedocify();
// OR
//app.UseRedocify(opt =>
//{
//    opt.LaunchRoute = "";
//    opt.SwaggerUrl = "swagger/customSwagger.json";
//});

app.UseAuthorization();
app.MapControllers();
app.Run();
