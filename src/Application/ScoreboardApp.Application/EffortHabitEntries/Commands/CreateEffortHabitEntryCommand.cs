using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands
{
    public sealed record CreateEffortHabitEntryCommand : IRequest<CreateEffortHabitEntryCommandResponse>
    {
        public double Effort { get; init; }
        public double? SessionGoal { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed record CreateEffortHabitEntryCommandResponse : IMapFrom<EffortHabitEntry>
    {
        public Guid Id { get; init; }
        public double Effort { get; init; }
        public double? SessionGoal { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed class CreateEffortHabitEntryCommandHandler : IRequestHandler<CreateEffortHabitEntryCommand, CreateEffortHabitEntryCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateEffortHabitEntryCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<CreateEffortHabitEntryCommandResponse> Handle(CreateEffortHabitEntryCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var entryEntity = new EffortHabitEntry()
            {
                Effort = request.Effort,
                SessionGoal = request.SessionGoal,
                EntryDate = request.EntryDate,
                HabitId = request.HabitId,
                UserId = currentUserId
            };

            _context.EffortHabitEntries.Add(entryEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateEffortHabitEntryCommandResponse>(entryEntity);
        }
    }
}
