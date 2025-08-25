using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceApplication.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/Error")]
        [HttpGet]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            return Problem(
                detail: exception?.Message,
                title: "An error occurred"
            );
        }
    }
}
