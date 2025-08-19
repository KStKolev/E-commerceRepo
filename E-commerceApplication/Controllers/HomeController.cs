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

        [HttpGet("info")]
        public string GetInfo()
        {
            _logger.LogInformation("GetInfo method called");
            return "Hello world";
        }

        [HttpGet("id/{id}")]
        public int GetId(int id) 
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID must be greater than zero.");
            }

            _logger.LogInformation("GetId method called with id: {Id}", id);
            return id;
        }
    }
}