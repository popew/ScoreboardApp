using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.Commons.Queries
{
    public interface IPagedQuery
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
