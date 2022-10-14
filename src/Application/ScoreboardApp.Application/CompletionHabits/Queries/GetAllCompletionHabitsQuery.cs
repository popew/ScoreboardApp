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

namespace ScoreboardApp.Application.CompletionHabits.Queries
{
    public sealed record GetAllCompletionHabitsQuery : IRequest<IList<CompletionHabitDTO>>
    {
        public Guid HabitTrackerId { get; init; }
    }

    public sealed class GetAllCompletionHabitsQueryHandler : IRequestHandler<GetAllCompletionHabitsQuery, IList<CompletionHabitDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCompletionHabitsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IList<CompletionHabitDTO>> Handle(GetAllCompletionHabitsQuery request, CancellationToken cancellationToken)
        {
            return await _context.CompletionHabits
                                .AsNoTracking()
                                .Where(x => x.HabitTrackerId == request.HabitTrackerId)
                                .ProjectTo<CompletionHabitDTO>(_mapper.ConfigurationProvider)
                                .OrderBy(ht => ht.Title)
                                .ToListAsync(cancellationToken);
        }
    }
}
