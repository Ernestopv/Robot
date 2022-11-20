using Microsoft.OpenApi.Models;
using Robot.Infrastructure.Settings;
using Robot.Services.Implementation;
using Robot.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);



// Add services to the container.
var services = builder.Services;
services.AddOptions();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Robot.Api", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.EnableAnnotations();
});

var appSettings = builder.Configuration.Get<AppSettings>();

if (!appSettings!.IsDevelopment)
{
    builder.WebHost.ConfigureKestrel(
        options => options.ListenAnyIP(3000)
    );
}

services.AddSingleton(appSettings);
services.AddControllersWithViews();
services.AddEndpointsApiExplorer();
services.AddSingleton<IMotorService, MotorService>();
services.AddScoped<IUtilitesService, UtilitiesService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Robot.Api v1");
    });
}
// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();


app.MapFallbackToFile("index.html"); ;

app.Run();
