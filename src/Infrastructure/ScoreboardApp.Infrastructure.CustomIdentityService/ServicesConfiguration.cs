using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.CustomIdentityService
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddCustomIdentityService(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
