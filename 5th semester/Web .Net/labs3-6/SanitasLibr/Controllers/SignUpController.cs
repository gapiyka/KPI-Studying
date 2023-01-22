using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace SanitasLibr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignUpController : ControllerBase
    {
        private readonly ILogger<SignUpController> _logger;

        public SignUpController(ILogger<SignUpController> logger)
        {
            _logger = logger;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult SignUp(string date, string time)
        {
            var str = Program.registry.SignUp(date, time);

            if (str.Contains("WRONG")) return BadRequest(str);
            if (str.Contains("database")) return Forbid(str);
            return Ok(str);
        }
    }
}
