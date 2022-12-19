using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.Queries
{
    public sealed record GetHabitTrackerQuery(Guid Id) : IRequest<HabitTrackerDTO>;

    public sealed class GetHabitTrackerQueryHandler : IRequestHandler<GetHabitTrackerQuery, HabitTrackerDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetHabitTrackerQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }


        public async Task<HabitTrackerDTO> Handle(GetHabitTrackerQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitTrackerEntity = await _context.HabitTrackers.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (habitTrackerEntity == null)
            {
                throw new NotFoundException(nameof(HabitTracker), request.Id);
            }

            return _mapper.Map<HabitTrackerDTO>(habitTrackerEntity);
        }
    }
}
