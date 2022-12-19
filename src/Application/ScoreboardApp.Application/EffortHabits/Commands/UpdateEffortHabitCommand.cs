using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Application.Habits.Commands
{
    public sealed record UpdateEffortHabitCommand : IRequest<UpdateEffortHabitCommandResponse>
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public string? Description { get; init; }

        public string? Unit { get; init; }
        public double? AverageGoal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed record UpdateEffortHabitCommandResponse : IMapFrom<EffortHabit>
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public string? Description { get; init; }

        public string? Unit { get; init; }
        public double? AverageGoal { get; init; }

        public EffortHabitSubtypeMapping Subtype { get; init; }
        public Guid HabitTrackerId { get; init; }
    }

    public sealed class UpdateEfforHabitCommandHandler : IRequestHandler<UpdateEffortHabitCommand, UpdateEffortHabitCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEfforHabitCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<UpdateEffortHabitCommandResponse> Handle(UpdateEffortHabitCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitEntity = await _context.EffortHabits
                                            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (habitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            habitEntity.Title = request.Title;
            habitEntity.Description = request.Description;
            habitEntity.Unit = request.Unit;
            habitEntity.AverageGoal = request.AverageGoal;
            habitEntity.Subtype = (EffortHabitSubtype)request.Subtype;
            habitEntity.HabitTrackerId = request.HabitTrackerId;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateEffortHabitCommandResponse>(habitEntity);
        }
    }
}
