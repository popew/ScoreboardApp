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
    public sealed record GetAllEffortHabitsQuery : IRequest<GetAllEffortHabitsQueryResponse>
    {
        public Guid HabitTrackerId { get; init; }
    }

    public sealed record GetAllEffortHabitsQueryResponse
    {
        public IList<EffortHabitDTO> EffortHabits { get; init; }
    }

    public sealed class GetAllEffortHabitsQueryHandler : IRequestHandler<GetAllEffortHabitsQuery, GetAllEffortHabitsQueryResponse>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllEffortHabitsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetAllEffortHabitsQueryResponse> Handle(GetAllEffortHabitsQuery request, CancellationToken cancellationToken)
        {
            var effortHabitEntities = await _context.EffortHabits
                                                    .AsNoTracking()
                                                    .Where(x => x.Id == request.HabitTrackerId)
                                                    .ProjectTo<EffortHabitDTO>(_mapper.ConfigurationProvider)
                                                    .OrderBy(ht => ht.Title)
                                                    .ToListAsync(cancellationToken);

            return new GetAllEffortHabitsQueryResponse()
            {
                 EffortHabits = effortHabitEntities
            };
        }
    }
}
