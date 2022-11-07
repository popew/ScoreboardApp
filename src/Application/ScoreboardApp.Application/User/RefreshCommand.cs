using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public sealed record RefreshCommand() : IRequest<Result<RefreshCommandResponse, Error>>
    {
        public string Token { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
    }
    public sealed record RefreshCommandResponse() : IMapFrom<TokenResponse>
    {
        public string Token { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
    }

    public sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<RefreshCommandResponse, Error>>
    {
        private readonly IUserService _tokenService;
        private readonly IMapper _mapper;

        public RefreshCommandHandler(IUserService tokenService, IMapper mapper)
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