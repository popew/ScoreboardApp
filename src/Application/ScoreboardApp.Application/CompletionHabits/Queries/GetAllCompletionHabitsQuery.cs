using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.CompletionHabits.Queries
{
    public sealed record GetAllCompletionHabitsQuery() : IRequest<IList<CompletionHabitDTO>>
    {
        public Guid? HabitTrackerId { get; init; }
    }

    public sealed class GetAllCompletionHabitsQueryHandler : IRequestHandler<GetAllCompletionHabitsQuery, IList<CompletionHabitDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllCompletionHabitsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<IList<CompletionHabitDTO>> Handle(GetAllCompletionHabitsQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            return await _context.CompletionHabits
                                .AsNoTracking()
                                .Where(x => x.UserId == currentUserId)
                                .Where(x => request.HabitTrackerId == null || x.HabitTrackerId == request.HabitTrackerId)
                                .ProjectTo<CompletionHabitDTO>(_mapper.ConfigurationProvider)
                                .OrderBy(ht => ht.Title)
                                .ToListAsync(cancellationToken);
        }
    }
}
