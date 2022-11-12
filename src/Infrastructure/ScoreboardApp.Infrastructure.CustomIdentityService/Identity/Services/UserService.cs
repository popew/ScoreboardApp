using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;
using System.Security.Claims;

namespace ScoreboardApp.Infrastructure.Identity.Services
{
    public sealed class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly TokenSettings _tokenSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IOptions<TokenSettings> tokenSettings
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _tokenSettings = tokenSettings.Value;
        }
        
        /// <inheritdoc/>
        public async Task<Result<string, Error>> CreateUserAsync(ApplicationUser newUser, string password)
        {
            var identityResult = await _userManager.CreateAsync(newUser, password);

            if (!identityResult.Succeeded)
            {
                var error = new Error()
                {
                    Details = identityResult.Errors.ToDictionary(x => x.Code, x => x.Description)
                };

                return Result.Failure<string, Error>(error);
            }

            return Result.Success<string, Error>(newUser.Id);
        }

        public async Task RevokeUsersRefreshTokenAsync(ApplicationUser user)
        {
            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);
        }

        public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public Result ValidateRefreshToken(ApplicationUser? user, string refreshToken)
        {
            if (user?.RefreshTokenExpiryTime <= DateTime.UtcNow || user?.RefreshToken != refreshToken)
            {
                return Result.Failure( "Refresh token is not valid." );
            }

            return Result.Success();
        }

        public async Task<Result> SignInUserAsync(ApplicationUser user, string password)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            return Result.SuccessIf(signInResult.Succeeded, "Invalid username or password." );
        }

        public async Task<TokenResponse> GenerateTokensForUserAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            string token = await _tokenService.GenerateJwtTokenAsync(user, roles);
            var (refreshToken, refreshTokenExpiry) = await _tokenService.GenerateRefreshTokenAsync();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            await _userManager.UpdateAsync(user);

            return new TokenResponse()
            {
                Token = token,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry
            };
        }


        public async Task<ApplicationUser?> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<Result<ClaimsPrincipal?>> GetPrincipalFromTokenAsync(string jwtToken)
        {
            try
            {
                var (principal, _) = await _tokenService.ValidateTokenAsync(jwtToken);

                return Result.Success(principal);
            }
            catch (Exception ex)
            {
                return Result.Failure<ClaimsPrincipal?>($"Token validation failed due to error: {ex.Message}.");
            }
        }
    }
}