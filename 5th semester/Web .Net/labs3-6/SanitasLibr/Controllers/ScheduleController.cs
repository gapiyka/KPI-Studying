using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SanitasLibr.Models;

namespace SanitasLibr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Output> Get()
        {
            var str = Program.registry.Schedule();
            
            return Parser.ParseStringToModel(str);
        }
    }
}
