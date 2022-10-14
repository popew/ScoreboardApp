using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.Commons.Models;
using ScoreboardApp.Application.Commons.Queries;
using ScoreboardApp.Application.EffortHabitEntries.Queries;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.CompletionHabitEntries.Queries
{
    public sealed record GetAllCompletionHabitEntriesWithPaginationQuery : IPagedQuery, IRequest<PaginatedList<CompletionHabitEntryDTO>>
    {
        public Guid HabitId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public sealed class GetAllCompletionHabitEntriesWithPaginationQueryHandler : IRequestHandler<GetAllCompletionHabitEntriesWithPaginationQuery, PaginatedList<CompletionHabitEntryDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCompletionHabitEntriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CompletionHabitEntryDTO>> Handle(GetAllCompletionHabitEntriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.CompletionHabitEntries
                                .Where(x => x.HabitId == request.HabitId)
                                .OrderBy(x => x.EntryDate)
                                .ProjectTo<CompletionHabitEntryDTO>(_mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);


        }
    }
}
