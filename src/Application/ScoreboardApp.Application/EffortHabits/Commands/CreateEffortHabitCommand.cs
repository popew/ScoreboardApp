using ScoreboardApp.Application.Commons.Enums;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using MediatR;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record CreateEfforHabitCommand : IRequest<CreateEfforHabitCommandResponse>
    {
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public string? Unit { get; init; } = default!;
        public double? Goal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed record CreateEfforHabitCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public string? Unit { get; init; } = default!;
        public double? Goal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed class CreateEfforHabitCommandHandler : IRequestHandler<CreateEfforHabitCommand, CreateEfforHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateEfforHabitCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateEfforHabitCommandResponse> Handle(CreateEfforHabitCommand request, CancellationToken cancellationToken)
        {
            var habitEntity = new EffortHabit()
            {
                Title = request.Title,
                Description = request.Description,
                HabitTrackerId = request.HabitTrackerId
            };

            _context.EffortHabits.Add(habitEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateEfforHabitCommandResponse()
            {
                Id = habitEntity.Id,
                Title = habitEntity.Title,
                Description = habitEntity.Description,
                HabitTrackerId = habitEntity.HabitTrackerId
            };
        }
    }
}