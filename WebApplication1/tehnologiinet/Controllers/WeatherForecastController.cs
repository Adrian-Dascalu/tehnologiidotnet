using Microsoft.AspNetCore.Mvc;
using tehnologiinet.Interfaces;

namespace tehnologiinet.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMyFirstServiceInterface _myFirstService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IMyFirstServiceInterface myFirstService)
    {
        _logger = logger;
        _myFirstService = myFirstService;
    }

    [HttpGet(Name = "Hello")]
    public IActionResult Hello()
    {
        _myFirstService.Hello();
        return Ok("saluuuuuuuuuuuut!!!!!!!!!!!!");
    }
    
    [HttpPost]
    public IActionResult HelloPost([FromBody] string value)
    {
        return Ok("salut din POST.");
    }
    
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}