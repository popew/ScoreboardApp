using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record UpdateCompletionHabitCommand : IRequest<UpdateCompletionHabitCommandResponse>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Guid HabitTrackerId { get; init; }
    }

    public sealed record UpdateCompletionHabitCommandResponse : IMapFrom<CompletionHabit>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Guid HabitTrackerId { get; init; }
    }

    public sealed class UpdateCompletionHabitCommandHandler : IRequestHandler<UpdateCompletionHabitCommand, UpdateCompletionHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCompletionHabitCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<UpdateCompletionHabitCommandResponse> Handle(UpdateCompletionHabitCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitEntity = await _context.CompletionHabits
                                            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (habitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            habitEntity.Title = request.Title;
            habitEntity.Description = request.Description;
            habitEntity.HabitTrackerId = request.HabitTrackerId;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateCompletionHabitCommandResponse>(habitEntity);
        }
    }
}
