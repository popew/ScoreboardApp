using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;

namespace ScoreboardApp.Application.Authentication
{
    public sealed record RevokeCommand() : IRequest<UnitResult<Error>>
    {
        public string UserName { get; init; } = default!;
    }

    public sealed class RevokeCommandHandler : IRequestHandler<RevokeCommand, UnitResult<Error>>
    {
        private readonly IUserService _tokenService;
        private readonly IMapper _mapper;

        public RevokeCommandHandler(IUserService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UnitResult<Error>> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            var revokeRequest = new RevokeRequest()
            {
                UserName = request.UserName
            };

            var result = await _tokenService.Revoke(revokeRequest, cancellationToken);

            return result;
        }
    }
}