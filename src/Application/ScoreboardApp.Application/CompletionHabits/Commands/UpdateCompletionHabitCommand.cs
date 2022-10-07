using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;
using ScoreboardApp.Infrastructure.Persistence;

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

        public UpdateCompletionHabitCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateCompletionHabitCommandResponse> Handle(UpdateCompletionHabitCommand request, CancellationToken cancellationToken)
        {
            var habitEntity = await _context.CompletionHabits
                                            .FindAsync(new object[] { request.Id }, cancellationToken);

            if (habitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            habitEntity.Title = request.Title;
            habitEntity.Description = request.Description;
            habitEntity.HabitTrackerId = request.HabitTrackerId;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateEffortHabitCommandResponse>(habitEntity);
        }
    }
}
