using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace SanitasLibr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangeDoctorController : ControllerBase
    {
        private readonly ILogger<ChangeDoctorController> _logger;

        public ChangeDoctorController(ILogger<ChangeDoctorController> logger)
        {
            _logger = logger;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ChangeDoctor(string name)
        {
            var str = Program.registry.SetCurrentDoctor(name);

            return Ok(str);
        }
    }
}
