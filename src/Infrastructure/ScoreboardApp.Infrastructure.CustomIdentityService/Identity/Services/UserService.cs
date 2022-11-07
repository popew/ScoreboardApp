using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;

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

        public async Task<Result<TokenResponse, Error>> Authenticate(AuthenticationRequest request, CancellationToken cancellationToken = default!)
        {
            var signInResult = await SignInUser(request.UserName, request.Password);

            if (signInResult.IsFailure)
            {
                return Result.Failure<TokenResponse, Error>(signInResult.Error);
            }

            ApplicationUser user = signInResult.Value;

            var roles = await _userManager.GetRolesAsync(user);

            string token = await _tokenService.GenerateJwtTokenAsync(user, roles);
            string refreshToken = await _tokenService.GenerateRefreshTokenAsync();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            await _userManager.UpdateAsync(user);

            return Result.Success<TokenResponse, Error>(new TokenResponse()
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        public async Task<Result<TokenResponse, Error>> RefreshJwtToken(RefreshRequest request, CancellationToken cancellationToken = default!)
        {
            var (principal, _) = await _tokenService.ValidateTokenAsync(request.Token);

            if (principal is null)
            {
                return Result.Failure<TokenResponse, Error>(Errors.InvalidTokenError());
            }

            ApplicationUser? user = await GetUserByUserName(principal!.Identity!.Name!);

            if (user is null)
            {
                return Result.Failure<TokenResponse, Error>(Errors.UserNotFoundError(principal.Identity!.Name!));
            }

            var refreshTokenValidationResult = ValidateRefreshToken(user, request.RefreshToken);

            if (refreshTokenValidationResult.IsFailure)
            {
                return Result.Failure<TokenResponse, Error>(refreshTokenValidationResult.Error);
            }

            var roles = await _userManager.GetRolesAsync(user);

            string token = await _tokenService.GenerateJwtTokenAsync(user, roles);
            string refreshToken = await _tokenService.GenerateRefreshTokenAsync();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            await _userManager.UpdateAsync(user);

            return Result.Success<TokenResponse, Error>(new TokenResponse()
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        public async Task<UnitResult<Error>> Register(RegistrationRequest request, CancellationToken cancellationToken = default!)
        {
            ApplicationUser? existingUser = await GetUserByUserName(request.UserName);

            if (existingUser is not null)
            {
                return UnitResult.Failure(Errors.UserAlreadyExistsError(request.UserName));
            }

            ApplicationUser newUser = new()
            {
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = true // TODO: Add email verification at some point
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            return result.Succeeded
                ? UnitResult.Success<Error>()
                : UnitResult.Failure(Errors.RegistrationFailedError().WithDetails(result.Errors.Select(x => x.Description)));
        }

        public async Task<UnitResult<Error>> Revoke(RevokeRequest request, CancellationToken cancellationToken = default!)
        {
            ApplicationUser? user = await GetUserByUserName(request.UserName);

            if (user is null)
            {
                return UnitResult.Failure(Errors.UserNotFoundError(request.UserName));
            }

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return UnitResult.Success<Error>();
        }

        public async Task<ApplicationUser?> GetUserByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// Sign-in user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns user object on successful sign in.</returns>
        private async Task<Result<ApplicationUser, Error>> SignInUser(string username, string password)
        {
            ApplicationUser? user = await GetUserByUserName(username);

            if (user == null)
            {
                return Result.Failure<ApplicationUser, Error>(Errors.UserNotFoundError(username));
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            if (!signInResult.Succeeded)
            {
                return Result.Failure<ApplicationUser, Error>(Errors.SignInFailedError());
            }

            return Result.Success<ApplicationUser, Error>(user);
        }

        private UnitResult<Error> ValidateRefreshToken(ApplicationUser? user, string refreshToken)
        {
            if (user?.RefreshTokenExpiryTime <= DateTime.UtcNow || user?.RefreshToken != refreshToken)
            {
                return UnitResult.Failure(Errors.InvalidRefreshTokenError());
            }

            return UnitResult.Success<Error>();
        }
    }
}