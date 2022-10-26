using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public record RegisterCommand(string UserName, string Email, string Password) : IRequest<UnitResult<ErrorDTO>>;

    public sealed class RegisterRequestHandler : IRequestHandler<RegisterCommand, UnitResult<ErrorDTO>>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RegisterRequestHandler(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UnitResult<ErrorDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerRequest = new RegistrationRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email
            };

            var result = await _tokenService.Register(registerRequest);

            return result.MapError((error) => _mapper.Map<ErrorDTO>(error));
        }
    }
}