using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record DeleteCompletionHabitCommand(Guid Id) : IRequest
    {
        public Guid Id { get; set; } = Id;
    }

    public sealed class DeleteCompletionHabitCommandHandler : IRequestHandler<DeleteCompletionHabitCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCompletionHabitCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(DeleteCompletionHabitCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitEntity = await _context.CompletionHabits
                                .Where(x => x.Id == request.Id && x.UserId == currentUserId)
                                .SingleOrDefaultAsync(cancellationToken);

            if (habitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            _context.CompletionHabits.Remove(habitEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
