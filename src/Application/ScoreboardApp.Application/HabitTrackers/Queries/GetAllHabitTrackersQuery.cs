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

        public GetAllHabitTrackersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<HabitTrackerDTO>> Handle(GetAllHabitTrackersQuery request, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers
                .AsNoTracking()
                .ProjectTo<HabitTrackerDTO>(_mapper.ConfigurationProvider)
                .OrderBy(ht => ht.Title)
                .ToListAsync(cancellationToken);
        }
    }
}
