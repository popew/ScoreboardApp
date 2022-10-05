using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CreateEffortHabitEntryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CreateEffortHabitEntryCommandResponse> Handle(CreateEffortHabitEntryCommand request, CancellationToken cancellationToken)
        {
            var entryEntity = new EffortHabitEntry()
            {
                Effort = request.Effort,
                SessionGoal = request.SessionGoal,
                EntryDate = request.EntryDate,
                HabitId = request.HabitId
            };

            _context.EffortHabitEntries.Add(entryEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateEffortHabitEntryCommandResponse>(entryEntity);
        }
    }
}
