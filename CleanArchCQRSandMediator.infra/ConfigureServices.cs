using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.infra.Data;
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
                configuration.GetConnectionString("BlogDbContext")
                ?? throw new InvalidOperationException("Connection string" + " BlogDbContext not found.");

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // Register the IApplicationDbContext interface using BlogDbContext as the implementation
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<BlogDbContext>());

            return services;
        }
    }
}
