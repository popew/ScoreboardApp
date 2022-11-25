using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.CompletionHabitEntries.Commands
{
    public sealed record CreateCompletionHabitEntryCommand : IRequest<CreateCompletionHabitEntryCommandResponse>
    {
        public bool Completion { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed record CreateCompletionHabitEntryCommandResponse : IMapFrom<CompletionHabitEntry>
    {
        public Guid Id { get; init; }
        public bool Completion { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed class CreateCompletionHabitEntryCommandHandler : IRequestHandler<CreateCompletionHabitEntryCommand, CreateCompletionHabitEntryCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateCompletionHabitEntryCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<CreateCompletionHabitEntryCommandResponse> Handle(CreateCompletionHabitEntryCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var entryEntity = new CompletionHabitEntry()
            {
                Completion = request.Completion,
                EntryDate = request.EntryDate,
                HabitId = request.HabitId,
                UserId = currentUserId
            };

            _context.CompletionHabitEntries.Add(entryEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateCompletionHabitEntryCommandResponse>(entryEntity);
        }
    }
}
