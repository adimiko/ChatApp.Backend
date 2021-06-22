using ChatApp.Application.Services;
using ChatApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<IService>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }

        public static void AddApplication(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<WebsocketSenderService>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }
    }
}