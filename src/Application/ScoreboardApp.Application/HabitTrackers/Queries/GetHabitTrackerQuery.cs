using AutoMapper;
using MediatR;
using ScoreboardApp.Application.Commons.Exceptions;
using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Queries
{
    public sealed record GetHabitTrackerQuery(Guid Id) : IRequest<HabitTrackerDTO>
    {
        public Guid Id { get; init; } = Id;
    }

    public sealed class GetHabitTrackerQueryHandler : IRequestHandler<GetHabitTrackerQuery, HabitTrackerDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetHabitTrackerQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<HabitTrackerDTO> Handle(GetHabitTrackerQuery request, CancellationToken cancellationToken)
        {
            var habitTrackerEntity = await _context.HabitTrackers.FindAsync(new object[] { request.Id }, cancellationToken);

            if (habitTrackerEntity == null)
            {
                throw new NotFoundException(nameof(HabitTracker), request.Id);
            }

            return _mapper.Map<HabitTrackerDTO>(habitTrackerEntity);
        }
    }
}
