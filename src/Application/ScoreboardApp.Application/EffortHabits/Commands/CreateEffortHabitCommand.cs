using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record CreateEfforHabitCommand : IRequest<CreateEfforHabitCommandResponse>
    {
        public required string Title { get; init; }
        public string? Description { get; init; }

        public string? Unit { get; init; }
        public double? AverageGoal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed record CreateEfforHabitCommandResponse : IMapFrom<EffortHabit>
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public string? Description { get; init; }

        public string? Unit { get; init; }
        public double? AverageGoal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed class CreateEfforHabitCommandHandler : IRequestHandler<CreateEfforHabitCommand, CreateEfforHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateEfforHabitCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<CreateEfforHabitCommandResponse> Handle(CreateEfforHabitCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitEntity = new EffortHabit()
            {
                Title = request.Title,
                Description = request.Description,
                HabitTrackerId = request.HabitTrackerId,
                Unit = request.Unit,
                AverageGoal = request.AverageGoal,
                UserId = currentUserId,
                Subtype = (EffortHabitSubtype)request.Subtype
            };

            _context.EffortHabits.Add(habitEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateEfforHabitCommandResponse>(habitEntity);
        }
    }
}