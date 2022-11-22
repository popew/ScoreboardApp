using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public sealed record CreateHabitTrackerCommand : IRequest<CreateHabitTrackerCommandResponse>
    {
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    // Rationale: Commands shouldn't return any values, but it's nice to return the object back from the call
    public sealed record CreateHabitTrackerCommandResponse : IMapFrom<HabitTracker>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    public sealed class CreateHabitTrackerCommandHandler : IRequestHandler<CreateHabitTrackerCommand, CreateHabitTrackerCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateHabitTrackerCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            return _mapper.Map<CreateHabitTrackerCommandResponse>(habitTrackerEntity);
        }
    }
}
