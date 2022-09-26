using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Enums;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public sealed record UpdateHabitTrackerCommand : IRequest<UpdateHabitTrackerCommandResponse>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityLevel Priority { get; init; }
    }

    public sealed record UpdateHabitTrackerCommandResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public PriorityLevel Priority { get; init; }
    }

    public sealed class UpdateHabitTrackerCommandHandler : IRequestHandler<UpdateHabitTrackerCommand, UpdateHabitTrackerCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateHabitTrackerCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateHabitTrackerCommandResponse> Handle(UpdateHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            var habitTracker = await _context.HabitTrackers
                                        .FindAsync(new object[] { request.Id }, cancellationToken);

            if (habitTracker == null)
            {
                throw new NotFoundException(nameof(HabitTracker), request.Id);
            }

            _mapper.Map<UpdateHabitTrackerCommand, HabitTracker>(request, habitTracker);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateHabitTrackerCommandResponse>(habitTracker);
        }
    }
}
