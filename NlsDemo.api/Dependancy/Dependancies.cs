using NlsDemo.service.IService;
using NlsDemo.service.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NlsDemo.Dependancy
{
    public static class Dependancies
    {
        public static IServiceCollection RegisterServiceDependencies(this IServiceCollection services)
        {
            #region Service
            services.AddScoped<IUserService, UserService>();
            #endregion

            return services;
        }
    }
}
