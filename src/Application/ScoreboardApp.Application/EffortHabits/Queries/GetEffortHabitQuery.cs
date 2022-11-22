using AutoMapper;
using MediatR;
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

        public GetEffortHabitQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<EffortHabitDTO> Handle(GetEffortHabitQuery request, CancellationToken cancellationToken)
        {
            var effortHabitEntity = await _context.EffortHabits.FindAsync(new object[] { request.Id }, cancellationToken);

            if (effortHabitEntity == null)
            {
                throw new NotFoundException(nameof(EffortHabit), request.Id);
            }

            return _mapper.Map<EffortHabitDTO>(effortHabitEntity);
        }
    }
}
