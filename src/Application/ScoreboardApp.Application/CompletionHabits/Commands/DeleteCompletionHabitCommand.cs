using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record DeleteCompletionHabitCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public sealed class DeleteCompletionHabitCommandHandler : IRequestHandler<DeleteCompletionHabitCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCompletionHabitCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteCompletionHabitCommand request, CancellationToken cancellationToken)
        {
            var habitEntity = await _context.CompletionHabits
                                .Where(h => h.Id == request.Id)
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
