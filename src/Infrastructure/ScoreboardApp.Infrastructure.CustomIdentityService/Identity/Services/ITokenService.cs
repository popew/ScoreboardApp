using CSharpFunctionalExtensions;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services
{
    public interface ITokenService
    {
        Task<Result<TokenResponse>> Authenticate(AuthenticationRequest request);

        Task<Result<TokenResponse>> Refresh(RefreshRequest request);

        Task<Result> Revoke(RevokeRequest request);

        Task<Result> Register(RegistrationRequest request);
    }
}