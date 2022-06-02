using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.DomainEvents;
using Miru.Queuing;

namespace Miru;

public static class MiruRegistry
{
    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        return services
            .AddTransient<IInterceptor, DomainEventsInterceptor>()
            .AddTransient<IInterceptor, EnqueueEventsInterceptor>();
    }
}