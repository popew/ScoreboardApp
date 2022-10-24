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

        public async Task<Result<TokenResponse>> Authenticate(AuthenticationRequest request)
        {
            var signInResult = await SignInUser(request.UserName, request.Password);

            if (signInResult.IsFailure)
            {
                return Result.Failure<TokenResponse>(signInResult.Error);
            }

            ApplicationUser user = signInResult.Value;

            string token = await GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            await _userManager.UpdateAsync(user);

            return Result.Success<TokenResponse>(new TokenResponse()
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        public async Task<Result<TokenResponse>> Refresh(RefreshRequest request)
        {
            var getClaimsPrincipalResult = GetPrincipalFromExpiredToken(request.Token);
            
            if (getClaimsPrincipalResult.IsFailure)
            {
                return Result.Failure<TokenResponse>(getClaimsPrincipalResult.Error);
            }

            ClaimsPrincipal principal = getClaimsPrincipalResult.Value;

            ApplicationUser? user = await GetUserByEmail(principal.Identity!.Name!);

            if(user is null)
            {
                return Result.Failure<TokenResponse>(Errors.UserNotFoundError);
            }

            var refreshTokenValidationResult = ValidateRefreshToken(user, request.RefreshToken);

            if(refreshTokenValidationResult.IsFailure)
            {
                return Result.Failure<TokenResponse>(refreshTokenValidationResult.Error);
            }

            string token = await GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            await _userManager.UpdateAsync(user);

            return Result.Success(new TokenResponse()
            {
                Token = token,
                RefreshToken = refreshToken
            });

        }

        public async Task<Result> Register(RegistrationRequest request)
        {
            ApplicationUser? existingUser = await GetUserByEmail(request.UserName);

            if(existingUser is not null)
            {
                return Result.Failure<RegistrationResult>(Errors.UserAlreadyExistsError);
            }

            ApplicationUser newUser = new()
            {
                Email = request.Email,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            return result.Succeeded
                ? Result.Success()
                : Result.Failure(Errors.RegistrationFailedError);
        }

        public async Task<Result> Revoke(RevokeRequest request)
        {
            ApplicationUser? user = await GetUserByEmail(request.UserName);

            if(user is null)
            {
                return Result.Failure(Errors.UserNotFoundError);
            }

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);                

            return Result.Success();
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            byte[] secret = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
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
        private Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
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
                return Result.Failure<ClaimsPrincipal>(Errors.InvalidTokenError);
            }

            return Result.Success<ClaimsPrincipal>(principal);

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
        private async Task<Result<ApplicationUser>> SignInUser(string username, string password)
        {
            ApplicationUser? user = await GetUserByEmail(username);

            if (user == null)
            {
                return Result.Failure<ApplicationUser>(Errors.UserNotFoundError);
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            if (!signInResult.Succeeded)
            {
                return Result.Failure<ApplicationUser>(Errors.SignInFailedError);
            }

            return Result.Success<ApplicationUser>(user);
        }

        private Result ValidateRefreshToken(ApplicationUser? user, string refreshToken)                 
        {
            var errorList = new List<Result>();

            if(user?.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                errorList.Add(Result.Failure(Errors.RefreshTokenExpiredError));
            }

            if(user?.RefreshToken != refreshToken)
            {
                errorList.Add(Result.Failure(Errors.InvalidRefreshTokenError));
            }

            return errorList.Any()
                ? Result.Combine(errorList)
                : Result.Success();
        }
    }
}