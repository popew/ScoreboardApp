using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using MediatR;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public sealed record AuthenticateRequest(string UserName, string Password) : IRequest<Result<AuthenticateResponse, Error>>;

    public sealed record AuthenticateResponse(string Token, string RefreshToken);

    public sealed class AuthenticateRequestHandler : IRequestHandler<AuthenticateRequest, Result<AuthenticateResponse, Error>>
    {
        private readonly ITokenService _tokenService;

        public AuthenticateRequestHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Result<AuthenticateResponse, Error>> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            var authRequest = new AuthenticationRequest()
            {
                UserName = request.UserName,
                Password = request.Password
            };

            return (await _tokenService.Authenticate(authRequest)
                                       .Map((tokenResponse) => new AuthenticateResponse(tokenResponse.Token, tokenResponse.RefreshToken)));
        }
    }
}