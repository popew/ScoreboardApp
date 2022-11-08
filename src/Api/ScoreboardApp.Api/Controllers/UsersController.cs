using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Authentication;

namespace ScoreboardApp.Api.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateCommand request)
        {
            var response = await Mediator.Send(request);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand request)
        {
            await Mediator.Send(request);

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshCommand request)
        {
            var response = await Mediator.Send(request);

            return Ok(response);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RevokeCommand request)
        {
            await Mediator.Send(request);

            return Ok();
        }
    }
}