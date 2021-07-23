using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oibi.Demo.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> OnGet()
		{
			var rng = new Random();
			return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			}));
		}


		[HttpPost]
		public async Task<IActionResult> OnPost([FromBody] DataExample data)
		{
			return Ok(data);
		}

		[HttpPut]
		public async Task<IActionResult> OnPut([FromBody] DataExample data)
		{
			return Ok(data);
		}

		[HttpDelete]
		public async Task<IActionResult> OnDelete()
		{
			return Ok(new { Ok = true });
		}


		public class DataExample
		{
			[Required]
			public string Name { get; set; }
			public int Age { get; set; }
			public DateTime BirthDay { get; set; }
		}
	}
}