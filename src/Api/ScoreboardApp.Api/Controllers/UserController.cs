using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Authentication;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors;
using System.Reflection.Metadata.Ecma335;

namespace ScoreboardApp.Api.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode, result.Error);
            }

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode, result.Error);
            }

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            throw new NotImplementedException();
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
