using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands
{
    public sealed record UpdateEffortHabitEntryCommand : IRequest<UpdateEffortHabitEntryCommandResponse>
    {
        public Guid Id { get; init; }
        public double Effort { get; init; }
        public double? SessionGoal { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed record UpdateEffortHabitEntryCommandResponse : IMapFrom<EffortHabitEntry>
    {
        public Guid Id { get; init; }
        public double Effort { get; init; }
        public double? SessionGoal { get; init; }
        public DateOnly EntryDate { get; init; }
        public Guid HabitId { get; init; }
    }

    public sealed class UpdateEffortHabitEntryCommandHandler : IRequestHandler<UpdateEffortHabitEntryCommand, UpdateEffortHabitEntryCommandResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEffortHabitEntryCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<UpdateEffortHabitEntryCommandResponse> Handle(UpdateEffortHabitEntryCommand request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var entryEntity = await _context.EffortHabitEntries
                                            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabitEntry), request.Id);
            }

            entryEntity.Effort = request.Effort;
            entryEntity.SessionGoal = request.SessionGoal;
            entryEntity.EntryDate = request.EntryDate;
            entryEntity.HabitId = request.HabitId;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateEffortHabitEntryCommandResponse>(entryEntity);
        }
    }
}
