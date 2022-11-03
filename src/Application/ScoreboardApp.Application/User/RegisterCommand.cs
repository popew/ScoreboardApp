using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public record RegisterCommand() : IRequest<UnitResult<Error>>
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;

        public string Email { get; init; } = default!;
    }

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