using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public sealed record DeleteHabitTrackerCommand(Guid Id) : IRequest;

    public sealed class DeleteHabitTrackercommandHandler : IRequestHandler<DeleteHabitTrackerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteHabitTrackercommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(DeleteHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitTrackerEntity = await _context.HabitTrackers
                                .Where(ht => ht.Id == request.Id && ht.UserId == currentUserId)
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
