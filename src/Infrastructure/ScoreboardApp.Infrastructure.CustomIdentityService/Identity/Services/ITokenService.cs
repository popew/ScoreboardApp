using CSharpFunctionalExtensions;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns JWT token and refresh token pair.</returns>
        Task<Result<TokenResponse, Error>> Authenticate(AuthenticationRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result<TokenResponse, Error>> Refresh(RefreshRequest request);

        /// <summary>
        /// Revokes refresh token for user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UnitResult<Error>> Revoke(RevokeRequest request);

        Task<UnitResult<Error>> Register(RegistrationRequest request);
    }
}