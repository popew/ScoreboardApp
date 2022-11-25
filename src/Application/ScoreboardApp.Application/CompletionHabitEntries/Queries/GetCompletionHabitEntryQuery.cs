using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.CompletionHabitEntries.Queries
{
    public sealed record GetCompletionHabitEntryQuery(Guid Id) : IRequest<CompletionHabitEntryDTO>
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class GetCompletionHabitEntryQueryHandler : IRequestHandler<GetCompletionHabitEntryQuery, CompletionHabitEntryDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetCompletionHabitEntryQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<CompletionHabitEntryDTO> Handle(GetCompletionHabitEntryQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var entryEntity = await _context.CompletionHabitEntries
                                            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (entryEntity == null)
            {
                throw new NotFoundException(nameof(CompletionHabitEntry), request.Id);
            }

            return _mapper.Map<CompletionHabitEntryDTO>(entryEntity);
        }
    }
}
