using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;

namespace ScoreboardApp.Application.Authentication
{
    public sealed record AuthenticateCommand() : IRequest<AuthenticateCommandResponse>
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
    }

    public sealed record AuthenticateCommandResponse() : IMapFrom<TokenResponse>
    {
        public string Token { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;

        public DateTime RefreshTokenExpiry { get; init; }
    }

    public sealed class AuthenticateRequestHandler : IRequestHandler<AuthenticateCommand, AuthenticateCommandResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthenticateRequestHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<AuthenticateCommandResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.GetUserByUserNameAsync(request.UserName);

            if (user == null)
            {
                throw new UnauthorizedException();
            }

            var result = await _userService.SignInUserAsync(user, request.Password);

            if (result.IsFailure)
            {
                throw new UnauthorizedException(result.Error);
            }

            var tokenResponse = await _userService.GenerateTokensForUserAsync(user);

            return _mapper.Map<AuthenticateCommandResponse>(tokenResponse);
        }
    }
}