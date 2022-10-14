using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.CompletionHabits.Queries
{
    public sealed record GetCompletionHabitQuery(Guid Id) : IRequest<CompletionHabitDTO>
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class GetCompletionHabitQueryHandler : IRequestHandler<GetCompletionHabitQuery, CompletionHabitDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCompletionHabitQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CompletionHabitDTO> Handle(GetCompletionHabitQuery request, CancellationToken cancellationToken)
        {
            var habitEntity = await _context.CompletionHabits.FindAsync(new object[] { request.Id }, cancellationToken);

            if(habitEntity == null)
            {
                throw new NotFoundException(nameof(CompletionHabit), request.Id);
            }

            return _mapper.Map<CompletionHabitDTO>(habitEntity);
        }
    }
}
                                        