using System.Net;
using Microsoft.EntityFrameworkCore;
using snoapi;
using snoapi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<SnoDB>(options => options.UseNpgsql(builder.Configuration["SnoTestDB"]));

builder.Services.AddScoped<SnoWriterService<Event>>();
builder.Services.AddScoped<SnoWriterService<Project>>();
builder.Services.AddScoped<SnoWriterService<User>>();
//builder.Services.AddScoped<SnoWriterService<Event>>();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json");

//int sslPort = (int)builder.Configuration.GetValue(typeof(int), "SSLPort");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}



app.UseHttpsRedirection();



app.Use // add CSP
(
    async (context, next) =>
    {
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
        await next.Invoke();
    }
);

app.UseAuthorization();

app.MapControllers();

app.Run();
