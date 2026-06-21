using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.infra.Data;
using CleanArchCQRSandMediator.infra.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchCQRSandMediator.infra
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("ApplicationDbContext")
                ?? throw new InvalidOperationException("Connection string" + " ApplicationDbContext not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // Register the IApplicationDbContext interface using ApplicationDbContext as the implementation
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            

            return services;
        }
    }
}
