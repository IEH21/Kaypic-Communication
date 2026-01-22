using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web3_kaypic.Models;

namespace web3_kaypic.Controllers
{
    [ApiController] // IMPORTANT
    [Route("api/[controller]")]
    [Authorize] // API protégée
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = " Accès à l’API protégé !",
                user = User.Identity?.Name,
                auth = User.Identity?.IsAuthenticated
            });
        }

    }
}