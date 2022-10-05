using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Application.HabitTrackers.Queries;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.EffortHabits.Queries
{
    public sealed record GetAllEffortHabitsQuery : IRequest<IList<EffortHabitDTO>>
    {
        public Guid HabitTrackerId { get; init; }
    }
    public sealed class GetAllEffortHabitsQueryHandler : IRequestHandler<GetAllEffortHabitsQuery, IList<EffortHabitDTO>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllEffortHabitsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IList<EffortHabitDTO>> Handle(GetAllEffortHabitsQuery request, CancellationToken cancellationToken)
        {
            return await _context.EffortHabits
                                .AsNoTracking()
                                .Where(x => x.Id == request.HabitTrackerId)
                                .ProjectTo<EffortHabitDTO>(_mapper.ConfigurationProvider)
                                .OrderBy(ht => ht.Title)
                                .ToListAsync(cancellationToken);

            
        }
    }
}
