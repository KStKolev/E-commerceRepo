using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceApplication.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("info")]
        public string GetInfo()
        {
            _logger.LogInformation("GetInfo method called");
            return "Hello world";
        }
    }
}