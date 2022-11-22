using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.EffortHabitEntries.Queries
{
    public sealed record GetEffortHabitEntryQuery(Guid Id) : IRequest<EffortHabitEntryDTO>
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed record GetEffortHabitEntryQueryHandler : IRequestHandler<GetEffortHabitEntryQuery, EffortHabitEntryDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEffortHabitEntryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<EffortHabitEntryDTO> Handle(GetEffortHabitEntryQuery request, CancellationToken cancellationToken)
        {
            var entryEntity = await _context.EffortHabitEntries.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabitEntry), request.Id);
            }

            return _mapper.Map<EffortHabitEntryDTO>(entryEntity);
        }
    }
}
