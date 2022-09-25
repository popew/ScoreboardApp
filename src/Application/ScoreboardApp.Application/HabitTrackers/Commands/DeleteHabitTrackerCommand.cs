using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Commands
{
    public record DeleteHabitTrackerCommand : IRequest<DeleteHabitTrackerCommandResponse>
    {

    }

    public record DeleteHabitTrackerCommandResponse
    {

    }
}
