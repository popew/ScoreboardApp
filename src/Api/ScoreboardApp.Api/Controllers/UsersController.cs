using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Authentication;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors;

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
                return StatusCode(ErrorMapping.ErrorToStatusMapping[result.Error.Code], result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(ErrorMapping.ErrorToStatusMapping[result.Error.Code], result.Error);
            }

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(ErrorMapping.ErrorToStatusMapping[result.Error.Code], result.Error);
            }

            return Ok(result.Value);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RevokeCommand request)
        {
            var result = await Mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(ErrorMapping.ErrorToStatusMapping[result.Error.Code], result.Error);
            }

            return Ok();
        }
    }
}