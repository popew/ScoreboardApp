using AutoMapper;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.Commons.Extensions
{
    public static class ResultExtensions
    {
        public static Result<TOut, EOut> AutoMap<TIn, EIn, TOut, EOut>(this Result<TIn, EIn> result, IMapper mapper)
        {
            if(result.IsSuccess)
            {
                return Result.Success<TOut, EOut>(mapper.Map<TOut>(result.Value));
            }

            return Result.Failure<TOut, EOut>(mapper.Map<EOut>(result.Error));
        }
    }
}
