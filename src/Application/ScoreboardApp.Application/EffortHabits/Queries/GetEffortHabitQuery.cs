using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.EffortHabits.Queries
{
    public sealed record GetEffortHabitQuery(Guid Id) : IRequest<EffortHabitDTO>
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class GetEffortHabitQueryHandler : IRequestHandler<GetEffortHabitQuery, EffortHabitDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetEffortHabitQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<EffortHabitDTO> Handle(GetEffortHabitQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var effortHabitEntity = await _context.EffortHabits
                                                  .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (effortHabitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            return _mapper.Map<EffortHabitDTO>(effortHabitEntity);
        }
    }
}
