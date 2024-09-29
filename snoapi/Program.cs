using snoapi;
using snoapi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<SnoDB>();

builder.Services.AddScoped<SnoWriterService<Event>>();
builder.Services.AddScoped<SnoWriterService<Project>>();
builder.Services.AddScoped<SnoWriterService<User>>();
//builder.Services.AddScoped<SnoWriterService<Event>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.Use // add CSP
(
    async (context, next) => {
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
        await next.Invoke();
    }
);

app.UseAuthorization();

app.MapControllers();

app.Run();
