using MediatR;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record CreateCompletionHabitCommand : IRequest<CreateCompletionHabitCommandResponse>
    {
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Guid HabitTrackerId { get; init; }
    }

    public sealed record CreateCompletionHabitCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Guid HabitTrackerId { get; init; }
    }

    public sealed class CreateCompletionHabitCommandHandler : IRequestHandler<CreateCompletionHabitCommand, CreateCompletionHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateCompletionHabitCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateCompletionHabitCommandResponse> Handle(CreateCompletionHabitCommand request, CancellationToken cancellationToken)
        {
            var habitEntity = new CompletionHabit()
            {
                Title = request.Title,
                Description = request.Description,
                HabitTrackerId = request.HabitTrackerId
            };

            _context.CompletionHabits.Add(habitEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCompletionHabitCommandResponse()
            {
                Id = habitEntity.Id,
                Title = habitEntity.Title,
                Description = habitEntity.Description,
                HabitTrackerId = habitEntity.HabitTrackerId
            };
        }
    }
}
