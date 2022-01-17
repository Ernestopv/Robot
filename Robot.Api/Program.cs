using Robot.Services;
using Robot.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(
    options => options.ListenAnyIP(3000)
);

// Add services to the container.
var services = builder.Services;
services.AddControllersWithViews();
services.AddEndpointsApiExplorer();
services.AddSingleton<IMotorService, MotorService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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
