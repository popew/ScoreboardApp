using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.EffortHabitEntries.Queries;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.CompletionHabitEntries.Queries
{
    public sealed record GetCompletionHabitEntryQuery(Guid Id) : IRequest<CompletionHabitEntryDTO>
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class GetCompletionHabitEntryQueryHandler : IRequestHandler<GetCompletionHabitEntryQuery, CompletionHabitEntryDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCompletionHabitEntryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CompletionHabitEntryDTO> Handle(GetCompletionHabitEntryQuery request, CancellationToken cancellationToken)
        {
            var entryEntity = await _context.CompletionHabitEntries.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(CompletionHabitEntry), request.Id);
            }

            return _mapper.Map<CompletionHabitEntryDTO>(entryEntity);
        }
    }
}
