using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContosoPizza.Controllers
{
    /// <summary>
    /// Weather Forecast Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        /// <summary>
        /// The summaries
        /// </summary>
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                var rng = new Random();
                List<WeatherForecast> WeatherForecastList = new List<WeatherForecast>();
                var dateCount = 0;

                while (dateCount < 7)
                {
                    var currentKeyWeatherForecast = new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(dateCount),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    };

                    WeatherForecastList.Add(currentKeyWeatherForecast);
                }

                return WeatherForecastList.ToArray();
            }
            catch
            {
                Console.WriteLine("Exception occurred.");
            }

            return null;
        }
    }
}
