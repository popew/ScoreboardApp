using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record DeleteEffortHabitCommand(Guid Id) : IRequest
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class DeleteEffortHabitCommandHandler : IRequestHandler<DeleteEffortHabitCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteEffortHabitCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(DeleteEffortHabitCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitEntity = await _context.EffortHabits
                                .Where(x => x.Id == request.Id && x.UserId == currentUserId)
                                .SingleOrDefaultAsync(cancellationToken);

            if (habitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            _context.EffortHabits.Remove(habitEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
