using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Enums;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public sealed record UpdateHabitTrackerCommand : IRequest<UpdateHabitTrackerCommandResponse>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    // Rationale: Commands shouldn't return any values, but it's nice to return the object back from the call
    public sealed record UpdateHabitTrackerCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    public sealed class UpdateHabitTrackerCommandHandler : IRequestHandler<UpdateHabitTrackerCommand, UpdateHabitTrackerCommandResponse>
    {
        private readonly IApplicationDbContext _context;

        public UpdateHabitTrackerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateHabitTrackerCommandResponse> Handle(UpdateHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            var habitTracker = await _context.HabitTrackers
                                        .FindAsync(new object[] { request.Id }, cancellationToken);

            if (habitTracker == null)
            {
                throw new NotFoundException(nameof(HabitTracker), request.Id);
            }

            habitTracker.Title = request.Title;
            habitTracker.Priority = (Priority)request.Priority;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateHabitTrackerCommandResponse()
            {
                Id = habitTracker.Id,
                Title = habitTracker.Title,
                Priority = (PriorityMapping)habitTracker.Priority
            };
        }
    }
}
