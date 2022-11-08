using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;

namespace ScoreboardApp.Application.Authentication
{
    public record RegisterCommand() : IRequest
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;

        public string Email { get; init; } = default!;
    }

    public sealed class RegisterRequestHandler : IRequestHandler<RegisterCommand, Unit>
    {
        private readonly IUserService _userService;

        public RegisterRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? existingUser = await _userService.GetUserByUserNameAsync(request.UserName);

            if (existingUser is not null)
            {
                throw new ConflictException();
            }

            ApplicationUser newUser = new()
            {
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = true // TODO: Add email verification at some point
            };

            var createUserResult = await _userService.CreateUserAsync(newUser, request.Password);

            if (createUserResult.IsFailure)
            {
                throw new ValidationException(createUserResult.Error.Details.ToDictionary(x => x.Key, x => new string[] { x.Value } ));
            }

            return Unit.Value;
        }
    }
}