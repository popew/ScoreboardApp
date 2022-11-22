using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands
{
    public sealed record DeleteEffortHabitEntryCommand(Guid Id) : IRequest<Unit>
    {
        public Guid Id { get; init; }
    }


    public sealed class DeleteEffortHabitEntryCommandHandler : IRequestHandler<DeleteEffortHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteEffortHabitEntryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEffortHabitEntryCommand request, CancellationToken cancellationToken)
        {
            var entryEntity = await _context.EffortHabitEntries
                    .Where(h => h.Id == request.Id)
                    .SingleOrDefaultAsync(cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabitEntry), request.Id);
            }

            _context.EffortHabitEntries.Remove(entryEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
