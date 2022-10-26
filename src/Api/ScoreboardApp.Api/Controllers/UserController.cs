using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Authentication;

namespace ScoreboardApp.Api.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode, result.Error);
            }

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode, result.Error);
            }

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode, result.Error);
            }

            return Ok();
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RevokeCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode, result.Error);
            }

            return Ok();
        }
    }
}