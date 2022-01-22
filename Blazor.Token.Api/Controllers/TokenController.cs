using Blazor.Token.Api.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blazor.Token.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenRepository _repository;

        public TokenController(ITokenRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody]TokenRequest request)
        {
            try
            {
                var response = await _repository.VerifyAsync(request);
                
                if (response == null)
                    return Unauthorized("Invalid credentials");

                return Ok(response);
            }
            catch(Exception x)
            {
                return BadRequest(x.Message);
            }

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] TokenRequest request)
        {
            try
            {
                await _repository.CreateAsync(request);
                return Ok();
            }
            catch (Exception x)
            {
                return BadRequest(x.Message);
            }
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<string>> Refresh([FromBody] string refreshToken)
        {
            return BadRequest("Not supported yet");
        }
    }
}
