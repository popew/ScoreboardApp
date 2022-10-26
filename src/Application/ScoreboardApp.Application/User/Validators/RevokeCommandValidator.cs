using FluentValidation;
using ScoreboardApp.Application.Authentication;

namespace ScoreboardApp.Application.User.Validators
{
    public sealed class RevokeCommandValidator : AbstractValidator<RevokeCommand>
    {
        public RevokeCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty();
        }
    }
}