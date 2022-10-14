using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.EffortHabitEntries.Commands;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CreateCompletionHabitEntryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CreateCompletionHabitEntryCommandResponse> Handle(CreateCompletionHabitEntryCommand request, CancellationToken cancellationToken)
        {
            var entryEntity = new CompletionHabitEntry()
            {
                Completion = request.Completion,
                EntryDate = request.EntryDate,
                HabitId = request.HabitId
            };

            _context.CompletionHabitEntries.Add(entryEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateCompletionHabitEntryCommandResponse>(entryEntity);
        }
    }
}
