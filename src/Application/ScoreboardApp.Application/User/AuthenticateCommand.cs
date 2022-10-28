using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public sealed record AuthenticateCommand(string UserName, string Password) : IRequest<Result<AuthenticateResponse, Error>>;

    public sealed record AuthenticateResponse(string Token, string RefreshToken) : IMapFrom<TokenResponse>;

    public sealed class AuthenticateRequestHandler : IRequestHandler<AuthenticateCommand, Result<AuthenticateResponse, Error>>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticateRequestHandler(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Result<AuthenticateResponse, Error>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var authRequest = new AuthenticationRequest()
            {
                UserName = request.UserName,
                Password = request.Password
            };

            var result = await _tokenService.Authenticate(authRequest, cancellationToken);

            return result.Map((serviceResponse) => _mapper.Map<AuthenticateResponse>(serviceResponse));
        }
    }
}