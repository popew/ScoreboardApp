using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.CompletionHabits.Queries
{
    public sealed record GetCompletionHabitQuery : IRequest<GetCompletionHabitQueryResponse>
    {

    }

    public sealed record GetCompletionHabitQueryResponse
    {

    }

    public sealed class GetCompletionHabitQueryHandler : IRequestHandler<GetCompletionHabitQuery, GetCompletionHabitQueryResponse>
    {
        public Task<GetCompletionHabitQueryResponse> Handle(GetCompletionHabitQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
