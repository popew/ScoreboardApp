using CSharpFunctionalExtensions;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns JWT token and refresh token pair.</returns>
        Task<Result<TokenResponse, Error>> Authenticate(AuthenticationRequest request, CancellationToken cancellationToken = default!);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result<TokenResponse, Error>> Refresh(RefreshRequest request, CancellationToken cancellationToken = default!);

        /// <summary>
        /// Revokes refresh token for user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UnitResult<Error>> Revoke(RevokeRequest request, CancellationToken cancellationToken = default!);

        Task<UnitResult<Error>> Register(RegistrationRequest request, CancellationToken cancellationToken = default!);
    }
}