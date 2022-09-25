using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Enums;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public record CreateHabitTrackerCommand : IRequest<CreateHabitTrackerCommandResponse>
    {
        public string Title { get; init; } = default!;
        public PriorityLevel Priority { get; init; }
    }

    public record CreateHabitTrackerCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityLevel PriorityLevel { get; init; }
    }

    public class CreateHabitTrackerCommandHandler : IRequestHandler<CreateHabitTrackerCommand, CreateHabitTrackerCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateHabitTrackerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CreateHabitTrackerCommandResponse> Handle(CreateHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            var habitTrackerEntity = _mapper.Map<HabitTracker>(request);

            _context.HabitTrackers.Add(habitTrackerEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateHabitTrackerCommandResponse>(habitTrackerEntity);
        }
    }
}
