using Microsoft.AspNetCore.Mvc;
using System;

namespace Security.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public dynamic Index()
        {
            return new
            {
                health = true,
                name = "Security",
                timestamp = DateTime.Now.ToString()
            };
        }
    }
}