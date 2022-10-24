using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public record RegisterRequest(string UserName, string Email, string Password) : IRequest<UnitResult<Error>>;

    public sealed class RegisterRequestHandler : IRequestHandler<RegisterRequest, UnitResult<Error>>
    {
        private readonly ITokenService _tokenService;

        public RegisterRequestHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public Task<UnitResult<Error>> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var registerRequest = new RegistrationRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email
            };

            return _tokenService.Register(registerRequest);
        }
    }
}