using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.EffortHabits.Queries
{
    public sealed class GetAllEffortHabitsQuery : IRequest<GetAllEffortHabitsQueryResponse>
    {

    }

    public sealed class GetAllEffortHabitsQueryResponse
    {

    }

    public sealed class GetAllEffortHabitsQueryHandler : IRequestHandler<GetAllEffortHabitsQuery, GetAllEffortHabitsQueryResponse>
    {
        public Task<GetAllEffortHabitsQueryResponse> Handle(GetAllEffortHabitsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
