using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.DTOs;

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
        private readonly ICurrentUserService _currentUserService;

        public GetAllEffortHabitsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<IList<EffortHabitDTO>> Handle(GetAllEffortHabitsQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            return await _context.EffortHabits
                                .AsNoTracking()
                                .Where(x => x.HabitTrackerId == request.HabitTrackerId && x.UserId == currentUserId)
                                .ProjectTo<EffortHabitDTO>(_mapper.ConfigurationProvider)
                                .OrderBy(ht => ht.Title)
                                .ToListAsync(cancellationToken);


        }
    }
}
