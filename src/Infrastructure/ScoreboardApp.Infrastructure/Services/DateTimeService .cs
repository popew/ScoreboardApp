using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Infrastructure.Services
{
    internal class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
