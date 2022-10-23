using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors
{
    public static class Errors
    {
        public static readonly Error UserNotFoundError = new() { Code = "user_not_found", Message = "User with given username does not exist." };
        
        public static readonly Error SignInFailedError = new() { Code = "cannot_sign_in", Message = "Error while trying to sign in. Validate user credentials." };

        public static readonly Error InvalidTokenError = new() { Code = "invalid_token", Message = "Token is not valid." };

        public static readonly Error RefreshTokenExpiredError = new() { Code = "refresh_token_expired", Message = "Refresh token expired." };

        public static readonly Error InvalidRefreshTokenError = new() { Code = "invalid_refresh_token", Message = "Refresh token is not valid" };
    }
}