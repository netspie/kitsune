using Microsoft.Extensions.DependencyInjection;

namespace Manabu.UI.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddTypes(
        this IServiceCollection services, Type[] types, ServiceLifetime lifetime)
    {
        if (types == null || types.Length == 0)
            return;

        foreach (var type in types)
            services.Add(new ServiceDescriptor(type, type, lifetime));
    }
}
