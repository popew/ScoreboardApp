using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddApplicationservices(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
