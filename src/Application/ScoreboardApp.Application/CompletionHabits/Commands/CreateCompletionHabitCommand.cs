using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record CreateCompletionHabitCommand : IRequest<CreateCompletionHabitCommandResponse>
    {
        public required string Title { get; init; }
        public string? Description { get; init; }

        public Guid HabitTrackerId { get; init; }
    }

    public sealed record CreateCompletionHabitCommandResponse : IMapFrom<CompletionHabit>
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public string? Description { get; init; }

        public Guid HabitTrackerId { get; init; }
    }

    public sealed class CreateCompletionHabitCommandHandler : IRequestHandler<CreateCompletionHabitCommand, CreateCompletionHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateCompletionHabitCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<CreateCompletionHabitCommandResponse> Handle(CreateCompletionHabitCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitEntity = new CompletionHabit()
            {
                Title = request.Title,
                Description = request.Description,      
                HabitTrackerId = request.HabitTrackerId,
                UserId = currentUserId
            };

            _context.CompletionHabits.Add(habitEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateCompletionHabitCommandResponse>(habitEntity);
        }
    }
}
