using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Api.IntegrationTests.UserController.TestData
{
    public record IdentityErrorModel(string Code, string Message);
    public class IdentityServerErrors
    {
        public static class Password
        {
            public static readonly IdentityErrorModel PasswordRequiresUpper = new("PasswordRequiresUpper", "Passwords must have at least one uppercase ('A'-'Z').");
            public static readonly IdentityErrorModel PasswordRequiresDigit = new("PasswordRequiresDigit", "Passwords must have at least one digit ('0'-'9').");
            public static readonly IdentityErrorModel PasswordRequiresNonAlphanumeric = new("PasswordRequiresNonAlphanumeric", "Passwords must have at least one non alphanumeric character.");
            public static readonly IdentityErrorModel PasswordTooShort = new("PasswordTooShort", "Passwords must be at least 6 characters.");
        }

    }

}
