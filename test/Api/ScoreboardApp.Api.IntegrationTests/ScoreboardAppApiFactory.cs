using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Api.IntegrationTests
{
    public class ScoreboardAppApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
