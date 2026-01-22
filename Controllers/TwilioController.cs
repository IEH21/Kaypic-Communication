using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twilio.Jwt.AccessToken;

namespace web3_kaypic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TwilioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("token")]
        public IActionResult GetToken(string identity)
        {
            var accountSid = _configuration["TwilioSettings:AccountSId"];
            var apiKey = _configuration["TwilioSettings:ApiKey"];
            var apiSecret = _configuration["TwilioSettings:ApiSecret"];

            // Sécurité : Twilio non configuré → pas de crash
            if (string.IsNullOrWhiteSpace(accountSid) ||
                string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(apiSecret))
            {
                return BadRequest("Twilio is not configured.");
            }

            var grant = new VideoGrant { Room = "SalleDemo" };

            var token = new Token(
                accountSid,
                apiKey,
                apiSecret,
                identity,
                grants: new HashSet<IGrant> { grant }
            );

            return Ok(token.ToJwt());
        }
    }
}