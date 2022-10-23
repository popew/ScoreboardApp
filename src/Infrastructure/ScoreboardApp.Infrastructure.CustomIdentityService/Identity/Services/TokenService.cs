using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScoreboardApp.Infrastructure.Identity.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenSettings _tokenSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<TokenSettings> tokenOptions
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenSettings = tokenOptions.Value;
        }

        public async Task<Result<TokenResponse, Error>> Authenticate(AuthenticationRequest request)
        {
            var signInResult = await SignInUser(request.UserName, request.Password);

            if (signInResult.IsFailure)
            {
                return Result.Failure<TokenResponse, Error>(signInResult.Error);
            }

            ApplicationUser user = signInResult.Value;

            string token = await GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            await _userManager.UpdateAsync(user);

            return Result.Success<TokenResponse, Error>(new TokenResponse()
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        public async Task<Result<TokenResponse, Error>> Refresh(RefreshRequest request)
        {
            var getClaimsPrincipalResult = GetPrincipalFromExpiredToken(request.Token);
            
            if (getClaimsPrincipalResult.IsFailure)
            {
                return Result.Failure<TokenResponse, Error>(getClaimsPrincipalResult.Error);
            }

            ClaimsPrincipal principal = getClaimsPrincipalResult.Value;

            ApplicationUser? user = await GetUserByEmail(principal.Identity!.Name!);

            if(user is null)
            {
                return Result.Failure<TokenResponse, Error>(Errors.UserNotFoundError);
            }

            var refreshTokenValidationResult = ValidateRefreshToken(user, request.RefreshToken);

            if(refreshTokenValidationResult.IsFailure)
            {
                return Result.Failure<TokenResponse, Error>(refreshTokenValidationResult.Error);
            }

            string token = await GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            await _userManager.UpdateAsync(user);

            return Result.Success<TokenResponse, Error>(new TokenResponse()
            {
                Token = token,
                RefreshToken = refreshToken
            });

        }

        public Task<Result<RegistrationResult, Error>> Register(RegistrationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UnitResult<Error>> Revoke(RevokeRequest request)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            byte[] secret = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.Expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature),
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Validates the token and returns its ClaimsPrincipal (user).
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns ClaimsPrincipal if input token is valid.</returns>
        private Result<ClaimsPrincipal, Error> GetPrincipalFromExpiredToken(string token)
        {
            byte[] secret = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _tokenSettings.Issuer,
                ValidAudience = _tokenSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                NameClaimType = ClaimTypes.NameIdentifier,
                RequireSignedTokens = true,
                RequireExpirationTime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) ||
                principal is null)
            {
                return Result.Failure<ClaimsPrincipal, Error>(Errors.InvalidTokenError);
            }

            return Result.Success<ClaimsPrincipal, Error>(principal);

        }

        private async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Sign-in user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns user object on successful sign in.</returns>
        private async Task<Result<ApplicationUser, Error>> SignInUser(string username, string password)
        {
            ApplicationUser? user = await GetUserByEmail(username);

            if (user == null)
            {
                return Result.Failure<ApplicationUser, Error>(Errors.UserNotFoundError);
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            if (!signInResult.Succeeded)
            {
                return Result.Failure<ApplicationUser, Error>(Errors.SignInFailedError);
            }

            return Result.Success<ApplicationUser, Error>(user);
        }

        private UnitResult<Error> ValidateRefreshToken(ApplicationUser? user, string refreshToken)
        {
            var errorList = new List<Error>();

            if(user?.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                errorList.Add(Errors.RefreshTokenExpiredError);
            }

            if(user?.RefreshToken != refreshToken)
            {
                errorList.Add(Errors.InvalidRefreshTokenError);
            }

            return errorList.Any()
                ? UnitResult.Failure(Errors.InvalidRefreshTokenError.CausedByErrors(errorList))
                : UnitResult.Success<Error>();
        }
    }
}