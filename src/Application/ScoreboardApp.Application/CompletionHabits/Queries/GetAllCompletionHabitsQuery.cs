using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.CompletionHabits.Queries
{
    public sealed record GetAllCompletionHabitsQuery : IRequest<GetAllCompletionHabitsQueryResponse>
    {
    }
    public sealed record GetAllCompletionHabitsQueryResponse
    {

    }

    public sealed class GetAllCompletionHabitsQueryHandler : IRequestHandler<GetAllCompletionHabitsQuery, GetAllCompletionHabitsQueryResponse>
    {
        public Task<GetAllCompletionHabitsQueryResponse> Handle(GetAllCompletionHabitsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
