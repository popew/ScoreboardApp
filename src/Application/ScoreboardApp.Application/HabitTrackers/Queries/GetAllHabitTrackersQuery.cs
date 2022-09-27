using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Queries
{
    public sealed record GetAllHabitTrackersQuery : IRequest<GetAllHabitTrackersResponse>
    {

    }

    public sealed record GetAllHabitTrackersResponse
    {
        public IList<HabitTrackerDTO> HabitTrackers { get; init; }
    }

    public sealed class GetAllHabitTrackersQueryHandler : IRequestHandler<GetAllHabitTrackersQuery, GetAllHabitTrackersResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllHabitTrackersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAllHabitTrackersResponse> Handle(GetAllHabitTrackersQuery request, CancellationToken cancellationToken)
        {
            var habitTrackers = await _context.HabitTrackers
                .AsNoTracking()
                .ProjectTo<HabitTrackerDTO>(_mapper.ConfigurationProvider)
                .OrderBy(ht => ht.Title)
                .ToListAsync(cancellationToken);

            return new GetAllHabitTrackersResponse()
            {
                HabitTrackers = habitTrackers
            };
        }
    }
}
