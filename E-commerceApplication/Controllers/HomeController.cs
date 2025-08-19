using Microsoft.AspNetCore.Mvc;

namespace E_commerceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public string GetInfo()
        {
            _logger.LogInformation($"Called GetInfo, trace: {HttpContext.TraceIdentifier}");
            return "Hello world";
        }
    }
}