using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.CompletionHabitEntries.Commands
{
    public sealed record DeleteCompletionHabitEntryCommand(Guid Id) : IRequest;
    public class DeleteCompletionHabitEntryCommandHandler : IRequestHandler<DeleteCompletionHabitEntryCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCompletionHabitEntryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteCompletionHabitEntryCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var entryEntity = await _context.CompletionHabitEntries
                    .Where(x => x.Id == request.Id && x.UserId == currentUserId)
                    .SingleOrDefaultAsync(cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabitEntry), request.Id);
            }

            _context.CompletionHabitEntries.Remove(entryEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
