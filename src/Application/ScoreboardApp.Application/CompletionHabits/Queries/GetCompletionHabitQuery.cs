using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.CompletionHabits.Queries
{
    public sealed record GetCompletionHabitQuery(Guid Id) : IRequest<CompletionHabitDTO>;

    public sealed class GetCompletionHabitQueryHandler : IRequestHandler<GetCompletionHabitQuery, CompletionHabitDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetCompletionHabitQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<CompletionHabitDTO> Handle(GetCompletionHabitQuery request, CancellationToken cancellationToken)
        {
            string? currentUserId = _currentUserService.GetUserId()!;

            var habitEntity = await _context.CompletionHabits
                                            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == currentUserId, cancellationToken);

            if (habitEntity == null)
            {
                throw new NotFoundException(nameof(CompletionHabit), request.Id);
            }

            return _mapper.Map<CompletionHabitDTO>(habitEntity);
        }
    }
}
