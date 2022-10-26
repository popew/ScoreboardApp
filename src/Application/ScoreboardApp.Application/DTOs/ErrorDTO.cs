using Newtonsoft.Json;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.DTOs
{
    public sealed record ErrorDTO : IMapFrom<Error>
    {
        public string Code { get; init; } = default!;
        public string Message { get; init; } = default!;

        public IList<string> Details = new List<string>();

        [JsonIgnore]
        public int StatusCode { get; init; }
    }
}
