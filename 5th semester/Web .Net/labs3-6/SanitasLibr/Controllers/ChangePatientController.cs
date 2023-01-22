using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace SanitasLibr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangePatientController : ControllerBase
    {
        private readonly ILogger<ChangePatientController> _logger;

        public ChangePatientController(ILogger<ChangePatientController> logger)
        {
            _logger = logger;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ChangePatient(string name)
        {
            var str = Program.registry.SetCurrentPatient(name);

            return Ok(str);
        }
    }
}
