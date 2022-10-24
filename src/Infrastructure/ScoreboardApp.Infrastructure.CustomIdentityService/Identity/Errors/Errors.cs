using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors
{
    public static class Errors
    {
        public static readonly string UserNotFoundError = "User with given username does not exist.";
        
        public static readonly string SignInFailedError = "Error occured while trying to sign in. Validate user credentials.";

        public static readonly string InvalidTokenError = "Token is not valid.";

        public static readonly string RefreshTokenExpiredError = "Refresh token expired." ;

        public static readonly string InvalidRefreshTokenError = "Refresh token is not valid";

        public static readonly string UserAlreadyExistsError = "User with given username already exists.";

        public static readonly string RegistrationFailedError = "Error occured while trying to create new user";
    }
}                   