using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.CompletionHabitEntries.Commands
{
    public sealed record UpdateCompletionHabitEntryCommand : IRequest<UpdateCompletionHabitEntryCommandResponse>
    {
        public Guid Id { get; init; }
        public bool Completion { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed record UpdateCompletionHabitEntryCommandResponse
    {
        public Guid Id { get; init; }
        public bool Completion { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed class UpdateCompletionHabitEntryCommandHanlder : IRequestHandler<UpdateCompletionHabitEntryCommand, UpdateCompletionHabitEntryCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCompletionHabitEntryCommandHanlder(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UpdateCompletionHabitEntryCommandResponse> Handle(UpdateCompletionHabitEntryCommand request, CancellationToken cancellationToken)
        {
            var entryEntity = await _context.CompletionHabitEntries
                                            .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabitEntry), request.Id);
            }

            entryEntity.Completion = request.Completion;
            entryEntity.EntryDate = request.EntryDate;
            entryEntity.HabitId = request.HabitId;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateCompletionHabitEntryCommandResponse>(entryEntity);
        }
    }
}
