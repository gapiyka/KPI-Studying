using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SanitasLibr.Models;

namespace SanitasLibr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedCardController : ControllerBase
    {
        private readonly ILogger<MedCardController> _logger;

        public MedCardController(ILogger<MedCardController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Output> Get()
        {
            var str = Program.registry.GetMedCard();
            
            return Parser.ParseStringToModel(str);
        }
    }
}
