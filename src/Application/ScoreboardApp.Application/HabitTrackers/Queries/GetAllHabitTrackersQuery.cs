using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.DTOs;

namespace ScoreboardApp.Application.HabitTrackers.Queries
{
    public sealed record GetAllHabitTrackersQuery : IRequest<IList<HabitTrackerDTO>>
    {

    }

    public sealed class GetAllHabitTrackersQueryHandler : IRequestHandler<GetAllHabitTrackersQuery, IList<HabitTrackerDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllHabitTrackersQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IList<HabitTrackerDTO>> Handle(GetAllHabitTrackersQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            return await _context.HabitTrackers
                .AsNoTracking()
                .Where(x => x.UserId == currentUserId)
                .ProjectTo<HabitTrackerDTO>(_mapper.ConfigurationProvider)
                .OrderBy(ht => ht.Title)
                .ToListAsync(cancellationToken);
        }
    }
}
