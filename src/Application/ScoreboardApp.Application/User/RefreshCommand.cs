using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public sealed record RefreshCommand(string Token, string RefreshToken) : IRequest<Result<RefreshCommandResponse, Error>>;
    public sealed record RefreshCommandResponse(string Token, string RefreshToken) : IMapFrom<TokenResponse>;

    public sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<RefreshCommandResponse, Error>>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RefreshCommandHandler(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Result<RefreshCommandResponse, Error>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var refreshRequest = new RefreshRequest()
            {
                Token = request.Token,
                RefreshToken = request.RefreshToken
            };

            var result = await _tokenService.Refresh(refreshRequest, cancellationToken);

            return result.Map((serviceResponse) => _mapper.Map<RefreshCommandResponse>(serviceResponse));
        }
    }
}