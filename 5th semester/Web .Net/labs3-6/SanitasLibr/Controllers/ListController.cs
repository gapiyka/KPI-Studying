using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SanitasLibr.Models;

namespace SanitasLibr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListController : ControllerBase
    {
        private readonly ILogger<ListController> _logger;

        public ListController(ILogger<ListController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Output> Get()
        {
            var str = Program.registry.List();
            
            return Parser.ParseStringToModel(str);
        }
    }
}
