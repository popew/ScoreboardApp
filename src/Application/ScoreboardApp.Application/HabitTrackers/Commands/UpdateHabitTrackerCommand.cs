using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public sealed record UpdateHabitTrackerCommand : IRequest<UpdateHabitTrackerCommandResponse>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    // Rationale: Commands shouldn't return any values, but it's nice to return the object back from the call
    public sealed record UpdateHabitTrackerCommandResponse : IMapFrom<HabitTracker>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityMapping Priority { get; init; }
    }

    public sealed class UpdateHabitTrackerCommandHandler : IRequestHandler<UpdateHabitTrackerCommand, UpdateHabitTrackerCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateHabitTrackerCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<UpdateHabitTrackerCommandResponse> Handle(UpdateHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitTrackerEntity = await _context.HabitTrackers
                                        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _currentUserService.GetUserId(), cancellationToken);

            if (habitTrackerEntity == null)
            {
                throw new NotFoundException(nameof(HabitTracker), request.Id);
            }

            habitTrackerEntity.Title = request.Title;
            habitTrackerEntity.Priority = (Priority)request.Priority;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateHabitTrackerCommandResponse>(habitTrackerEntity);
        }
    }
}
