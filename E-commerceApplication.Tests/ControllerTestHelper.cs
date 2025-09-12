using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerceApplication.Tests
{
    public static class ControllerTestHelper
    {
        public static void SetUser(ControllerBase controllerBase, Guid userId)
        {
            string authType = "mock";

            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            ], authType));

            controllerBase.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
    }
}