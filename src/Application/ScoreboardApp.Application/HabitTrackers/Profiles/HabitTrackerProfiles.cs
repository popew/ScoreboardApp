using AutoMapper;
using ScoreboardApp.Application.HabitTrackers.Commands;
using ScoreboardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Profiles
{
    public class HabitTrackerProfiles : Profile
    {
        public HabitTrackerProfiles()
        {
            CreateMap<CreateHabitTrackerCommand, HabitTracker>();
            CreateMap<HabitTracker, CreateHabitTrackerCommandResponse>();

            CreateMap<UpdateHabitTrackerCommand, HabitTracker>();
            CreateMap<HabitTracker, UpdateHabitTrackerCommand>();
        }
    }
}
