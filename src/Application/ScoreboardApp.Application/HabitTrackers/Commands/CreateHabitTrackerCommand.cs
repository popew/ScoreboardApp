using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public sealed record CreateHabitTrackerCommand : IRequest<CreateHabitTrackerCommandResponse>
    {
        public required string Title { get; init; }
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
        private readonly ICurrentUserService _currentUserService;

        public CreateHabitTrackerCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<CreateHabitTrackerCommandResponse> Handle(CreateHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitTrackerEntity = new HabitTracker()
            {
                Title = request.Title,
                Priority = (Priority)request.Priority,
                UserId = currentUserId
            };

            _context.HabitTrackers.Add(habitTrackerEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateHabitTrackerCommandResponse>(habitTrackerEntity);
        }
    }
}
