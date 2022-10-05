using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.Commons.Models;
using ScoreboardApp.Application.Commons.Queries;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Infrastructure.Persistence;

namespace ScoreboardApp.Application.EffortHabitEntries.Queries
{
    public sealed record GetEffortHabitEntriesWithPaginationQuery : IPagedQuery, IRequest<GetEffortHabitEntriesWithPaginationQueryResponse>
    {
        public Guid HabitId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public sealed record GetEffortHabitEntriesWithPaginationQueryResponse
    {
        public PaginatedList<EffortHabitEntryDTO> PaginatedEffortHabitEntries { get; init; }
    }

    public sealed class GetEffortHabitEntriesWithPaginationQueryHandler : IRequestHandler<GetEffortHabitEntriesWithPaginationQuery, GetEffortHabitEntriesWithPaginationQueryResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEffortHabitEntriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetEffortHabitEntriesWithPaginationQueryResponse> Handle(GetEffortHabitEntriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var paginatedList = await _context.EffortHabitEntries
                                                .Where(x => x.HabitId == request.HabitId)
                                                .OrderBy(x => x.EntryDate)
                                                .ProjectTo<EffortHabitEntryDTO>(_mapper.ConfigurationProvider)
                                                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return new GetEffortHabitEntriesWithPaginationQueryResponse()
            {
                PaginatedEffortHabitEntries = paginatedList
            };

        }
    }
}
