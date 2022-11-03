using FluentValidation;
using ScoreboardApp.Application.Authentication;

namespace ScoreboardApp.Application.User.Validators
{
    public sealed class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.UserName)
                .NotEmpty();
        }
    }
}