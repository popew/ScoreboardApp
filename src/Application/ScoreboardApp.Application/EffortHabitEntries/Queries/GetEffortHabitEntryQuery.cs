using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.EffortHabitEntries.Queries
{
    public sealed record GetEffortHabitEntryQuery(Guid Id) : IRequest<EffortHabitEntryDTO>;

    public sealed record GetEffortHabitEntryQueryHandler : IRequestHandler<GetEffortHabitEntryQuery, EffortHabitEntryDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetEffortHabitEntryQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<EffortHabitEntryDTO> Handle(GetEffortHabitEntryQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var entryEntity = await _context.EffortHabitEntries
                                            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabitEntry), request.Id);
            }

            return _mapper.Map<EffortHabitEntryDTO>(entryEntity);
        }
    }
}
