using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands
{
    public sealed record DeleteEffortHabitEntryCommand(Guid Id) : IRequest<Unit>
    {
        public Guid Id { get; init; } = Id;
    }


    public sealed class DeleteEffortHabitEntryCommandHandler : IRequestHandler<DeleteEffortHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteEffortHabitEntryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteEffortHabitEntryCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var entryEntity = await _context.EffortHabitEntries
                                            .Where(x => x.Id == request.Id && x.UserId == currentUserId)
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
