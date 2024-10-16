using Microsoft.AspNetCore.Mvc;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SnusController : ControllerBase
    {
        private readonly ILogger<SnusController> mLogger;

        public SnusController(ILogger<SnusController> aLogger)
        {
            mLogger = aLogger;
        }

        [HttpGet(Name = "Dummy")]
        public string Get()
        {
            return "Hello, World!";
        }
    }
}
