using FluentValidation;
using ScoreboardApp.Application.Authentication;
using System.Text.RegularExpressions;

namespace ScoreboardApp.Application.User.Validators
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private static readonly Regex EmailRegex =
            new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));

        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .Must(BeAValidEmail).WithMessage("{PropertyValue} is not a valid email.");

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.UserName)
                .NotEmpty();
        }

        private bool BeAValidEmail(string email)
        {
            try
            {
                return EmailRegex.IsMatch(email);
            }
            catch            
            {
                return false;
            }
        }               
    }
}