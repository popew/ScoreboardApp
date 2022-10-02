using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record DeleteEffortHabitCommand(Guid Id) : IRequest
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class DeleteEffortHabitCommandHandler : IRequestHandler<DeleteEffortHabitCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteEffortHabitCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteEffortHabitCommand request, CancellationToken cancellationToken)
        {
            var habitEntity = await _context.EffortHabits
                                .Where(h => h.Id == request.Id)
                                .SingleOrDefaultAsync(cancellationToken);

            if (habitEntity == null)
            {
                throw new NotFoundException(nameof(HabitTracker), request.Id);
            }

            _context.EffortHabits.Remove(habitEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
