using CSharpFunctionalExtensions;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services
{
    public interface ITokenService
    {
        Task<Result<TokenResponse, Error>> Authenticate(AuthenticationRequest request);

        Task<Result<TokenResponse, Error>> Refresh(RefreshRequest request);

        Task<UnitResult<Error>> Revoke(RevokeRequest request);

        Task<Result<RegistrationResult, Error>> Register(RegistrationRequest request);
    }
}