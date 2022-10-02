using MediatR;
using ScoreboardApp.Application.Commons.Enums;
using ScoreboardApp.Infrastructure.Persistence;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record UpdateEffortHabitCommand : IRequest<UpdateEffortHabitCommandResponse>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public string? Unit { get; init; } = default!;
        public double? Goal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed record UpdateEffortHabitCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public string? Unit { get; init; } = default!;
        public double? Goal { get; init; }

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

        public Task<UpdateEffortHabitCommandResponse> Handle(UpdateEffortHabitCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
