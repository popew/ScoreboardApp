using MediatR;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public class CreateHabitTrackerCommand : IRequest<Guid>
    {
        public string Title { get; set; } = default!;
    }

    public class CreateHabitTrackerCommandHanlder : IRequestHandler<CreateHabitTrackerCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateHabitTrackerCommandHanlder(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateHabitTrackerCommand request, CancellationToken cancellationToken)
        {
            var habitTrackerEntity = new HabitTracker()
            {
                Title = request.Title
            };

            _context.HabitTrackers.Add(habitTrackerEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return habitTrackerEntity.Id;
        }
    }
}
