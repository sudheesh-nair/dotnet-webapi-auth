﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private const string ADMIN_ROLE = "Administrator";
        private const string USER_ROLE = "User";

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
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("name")]
        public string GetName() => String.Format("Authenticated - {0} has role {1}", 
            User.Identity.Name,
            User.IsInRole(ADMIN_ROLE) ? ADMIN_ROLE : USER_ROLE);

        [HttpGet]
        [Route("topsecret")]
        [Authorize(Roles = ADMIN_ROLE)]
        public string GetSecureMessageForAdmin() => String.Format("Happiness is a daily decision");
    }
}
