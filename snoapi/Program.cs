using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
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
builder.Services.AddScoped<JsonFileReadService>();

builder.Services.AddRateLimiter(limitter =>

    {
        limitter.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        limitter.AddFixedWindowLimiter(

            policyName: "fixed", options =>
            {
                options.PermitLimit = 100;
                options.Window = TimeSpan.FromMinutes(1);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 0;
            }

        );
    }
);



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





var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();

//app.UseMiddleware<IPValidatorMiddleware>();

app.UseRateLimiter();


builder.Configuration.AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json");


app.UseHttpsRedirection();



app.Use // add CSP
(
    async (context, next) =>
    {
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
        await next.Invoke();
    }
);



app.MapControllers().RequireRateLimiting("fixed");

app.Run();
