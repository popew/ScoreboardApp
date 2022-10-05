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
    public sealed record GetEffortHabitEntriesWithPaginationQuery : IPagedQuery, IRequest<PaginatedList<EffortHabitEntryDTO>>
    {
        public Guid HabitId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public sealed class GetEffortHabitEntriesWithPaginationQueryHandler : IRequestHandler<GetEffortHabitEntriesWithPaginationQuery, PaginatedList<EffortHabitEntryDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEffortHabitEntriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<EffortHabitEntryDTO>> Handle(GetEffortHabitEntriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.EffortHabitEntries
                                .Where(x => x.HabitId == request.HabitId)
                                .OrderBy(x => x.EntryDate)
                                .ProjectTo<EffortHabitEntryDTO>(_mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);


        }
    }
}
