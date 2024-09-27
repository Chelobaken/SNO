using Microsoft.AspNetCore.Mvc;

namespace snoapi.Controllers;

[ApiController]
[Route("/api")]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;

    public ApiController(ILogger<ApiController> logger)
    {
        _logger = logger;
    }

    //[HttpGet(Name = "GetWeatherForecast")]
    //public IEnumerable<WeatherForecast> Get()
   // {
       
    //}
}
