using Microsoft.AspNetCore.Mvc;

namespace E_commerceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public string GetInfo()
        {
            return "Hello world";
        }
    }
}