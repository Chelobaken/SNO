using Microsoft.EntityFrameworkCore;
using SNO.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<SnoDB>(options => options.UseNpgsql(builder.Configuration["ConnectionStrings:SnoTestDB"]));

builder.Services.AddScoped<SnoWriterService<Event>>();
builder.Services.AddScoped<SnoWriterService<Project>>();
builder.Services.AddScoped<SnoWriterService<User>>();


/* builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,

            ValidIssuer = "KubSTU-Science-Community-JWT-Issuer",
            ValidAudience = "KubSTU-Science-Community-JWT-API-Consumer",
            
            ValidAlgorithms = ["HS256"],
            ValidateLifetime = true,
            
            
        };

        options.MapInboundClaims = false;
    }); */




builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json");

//int sslPort = (int)builder.Configuration.GetValue(typeof(int), "SSLPort");


var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();

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



app.MapControllers();

app.Run();
