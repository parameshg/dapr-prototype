using Microsoft.AspNetCore.Mvc;

namespace Security.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public string Index()
        {
            return "Authentication and Authorization API";
        }
    }
}