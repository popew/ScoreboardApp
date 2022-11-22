using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record CreateCompletionHabitCommand : IRequest<CreateCompletionHabitCommandResponse>
    {
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Guid HabitTrackerId { get; init; }
    }

    public sealed record CreateCompletionHabitCommandResponse : IMapFrom<CompletionHabit>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Guid HabitTrackerId { get; init; }
    }

    public sealed class CreateCompletionHabitCommandHandler : IRequestHandler<CreateCompletionHabitCommand, CreateCompletionHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCompletionHabitCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            return _mapper.Map<CreateCompletionHabitCommandResponse>(habitEntity);
        }
    }
}
