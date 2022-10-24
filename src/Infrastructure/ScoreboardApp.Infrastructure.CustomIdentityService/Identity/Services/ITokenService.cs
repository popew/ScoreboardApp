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
        Task<Result<TokenResponse>> Authenticate(AuthenticationRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result<TokenResponse>> Refresh(RefreshRequest request);

        /// <summary>
        /// Revokes refresh token for user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result> Revoke(RevokeRequest request);

        Task<Result> Register(RegistrationRequest request);
    }
}