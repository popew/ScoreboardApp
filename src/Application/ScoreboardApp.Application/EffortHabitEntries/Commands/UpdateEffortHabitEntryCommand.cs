using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
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
    public sealed record UpdateEffortHabitEntryCommand : IRequest<UpdateEffortHabitEntryCommandResponse>
    {
        public Guid Id { get; init; }
        public double Effort { get; init; }
        public double? SessionGoal { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed record UpdateEffortHabitEntryCommandResponse : IMapFrom<EffortHabitEntry>
    {
        public Guid Id { get; init; }
        public double Effort { get; init; }
        public double? SessionGoal { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed class UpdateEffortHabitEntryCommandHandler : IRequestHandler<UpdateEffortHabitEntryCommand, UpdateEffortHabitEntryCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEffortHabitEntryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateEffortHabitEntryCommandResponse> Handle(UpdateEffortHabitEntryCommand request, CancellationToken cancellationToken)
        {
            var entryEntity = await _context.EffortHabitEntries
                                            .FindAsync(new object[] { request.Id }, cancellationToken);

            if(entryEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabitEntry), request.Id);
            }

            entryEntity.Effort = request.Effort;
            entryEntity.SessionGoal = request.SessionGoal;
            entryEntity.EntryDate = request.EntryDate;
            entryEntity.HabitId = request.HabitId;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateEffortHabitEntryCommandResponse>(entryEntity);
        }
    }
}
