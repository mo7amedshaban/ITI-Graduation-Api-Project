using Core.Interfaces;
using Infrastructure.Common;
using Infrastructure.Common.GenRepo;
using Infrastructure.interfaces;
using Infrastructure.Interfaces;
using Infrastructure.services;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();

         
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            
            services.AddHybridCache(options => options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(10), // Distributed( L2, L3)
                LocalCacheExpiration = TimeSpan.FromSeconds(30), // Local Memory L1
            });


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;


        }

    }
}
