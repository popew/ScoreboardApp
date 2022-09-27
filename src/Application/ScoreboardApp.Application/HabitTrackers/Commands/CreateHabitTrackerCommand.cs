using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Enums;
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
    public sealed record CreateHabitTrackerCommand : IRequest<CreateHabitTrackerCommandResponse>
    {
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    // Rationale: Commands shouldn't return any values, but it's nice to return the object back from the call
    public sealed record CreateHabitTrackerCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    public sealed class CreateHabitTrackerCommandHandler : IRequestHandler<CreateHabitTrackerCommand, CreateHabitTrackerCommandResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateHabitTrackerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CreateHabitTrackerCommandResponse> Handle(CreateHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            var habitTrackerEntity = new HabitTracker()
            {
                Title = request.Title,
                Priority = (Priority)request.Priority
            };

            _context.HabitTrackers.Add(habitTrackerEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateHabitTrackerCommandResponse()
            {
                Id = habitTrackerEntity.Id,
                Title = habitTrackerEntity.Title,
                Priority = (PriorityMapping)habitTrackerEntity.Priority
            };
        }
    }
}
