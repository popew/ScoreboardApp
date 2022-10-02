using MediatR;
using ScoreboardApp.Application.Commons.Enums;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using ScoreboardApp.Domain.Enums;


namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record UpdateEffortHabitCommand : IRequest<UpdateEffortHabitCommandResponse>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public string? Unit { get; init; } = default!;
        public double? AverageGoal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed record UpdateEffortHabitCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public string? Unit { get; init; } = default!;
        public double? AverageGoal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed class UpdateEfforHabitCommandHandler : IRequestHandler<UpdateEffortHabitCommand, UpdateEffortHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;

        public UpdateEfforHabitCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateEffortHabitCommandResponse> Handle(UpdateEffortHabitCommand request, CancellationToken cancellationToken)
        {
            var habitEntity = await _context.EffortHabits
                                            .FindAsync(new object[] {request.Id}, cancellationToken);

            if(habitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            habitEntity.Title = request.Title;
            habitEntity.Description = request.Description;
            habitEntity.Unit = request.Unit;
            habitEntity.AverageGoal = request.AverageGoal;
            habitEntity.Subtype = (EffortHabitSubtype)request.Subtype;
            habitEntity.HabitTrackerId = request.HabitTrackerId;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateEffortHabitCommandResponse()
            {
                Title = habitEntity.Title,
                Description = habitEntity.Description,
                Unit = habitEntity.Unit,
                AverageGoal = habitEntity.AverageGoal,
                Subtype = (EffortHabitSubtypeMapping)habitEntity.Subtype,
                HabitTrackerId = habitEntity.HabitTrackerId
            };
        }
    }
}
