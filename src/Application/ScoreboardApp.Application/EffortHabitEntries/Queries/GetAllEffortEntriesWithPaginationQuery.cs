using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.Commons.Models;
using ScoreboardApp.Application.Commons.Queries;
using ScoreboardApp.Application.HabitTrackers.DTOs;

namespace ScoreboardApp.Application.EffortHabitEntries.Queries
{
    public sealed record GetAllEffortEntriesWithPaginationQuery : IPaginationQuery, IRequest<PaginatedList<EffortHabitEntryDTO>>
    {
        public Guid? HabitId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public sealed class GetEffortHabitEntriesWithPaginationQueryHandler : IRequestHandler<GetAllEffortEntriesWithPaginationQuery, PaginatedList<EffortHabitEntryDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetEffortHabitEntriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedList<EffortHabitEntryDTO>> Handle(GetAllEffortEntriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            return await _context.EffortHabitEntries
                                .Where(x => x.UserId== currentUserId)
                                .Where(x => request.HabitId == null || x.HabitId == request.HabitId)
                                .OrderBy(x => x.EntryDate)
                                .ProjectTo<EffortHabitEntryDTO>(_mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);


        }
    }
}
