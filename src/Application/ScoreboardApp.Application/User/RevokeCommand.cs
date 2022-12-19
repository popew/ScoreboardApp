using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;

namespace ScoreboardApp.Application.Authentication
{
    public sealed record RevokeCommand() : IRequest
    {
        public required string UserName { get; init; }
    }

    public sealed class RevokeCommandHandler : IRequestHandler<RevokeCommand, Unit>
    {
        private readonly IUserService _userService;

        public RevokeCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.GetUserByUserNameAsync(request.UserName);

            if (user is null)
            {
                throw new NotFoundException();
            }

            await _userService.RevokeUsersRefreshTokenAsync(user);

            return Unit.Value;
        }
    }
}