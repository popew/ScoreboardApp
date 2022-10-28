using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public record RegisterCommand(string UserName, string Email, string Password) : IRequest<UnitResult<Error>>;

    public sealed class RegisterRequestHandler : IRequestHandler<RegisterCommand, UnitResult<Error>>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RegisterRequestHandler(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UnitResult<Error>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerRequest = new RegistrationRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email
            };

            var result = await _tokenService.Register(registerRequest, cancellationToken);

            return result;
        }
    }
}