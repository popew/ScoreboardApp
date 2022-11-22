using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public sealed record DeleteHabitTrackerCommand(Guid Id) : IRequest
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class DeleteHabitTrackercommandHandler : IRequestHandler<DeleteHabitTrackerCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteHabitTrackercommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            var habitTrackerEntity = await _context.HabitTrackers
                                .Where(ht => ht.Id == request.Id)
                                .SingleOrDefaultAsync(cancellationToken);

            if (habitTrackerEntity == null)
            {
                throw new NotFoundException(nameof(HabitTracker), request.Id);
            }

            _context.HabitTrackers.Remove(habitTrackerEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }


}
