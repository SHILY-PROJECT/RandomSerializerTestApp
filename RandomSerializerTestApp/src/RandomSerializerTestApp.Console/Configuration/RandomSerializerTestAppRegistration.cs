using Microsoft.Extensions.DependencyInjection;

namespace RandomSerializerTestApp.Core.Configuration;

public static class RandomSerializerTestAppRegistration
{
    public static IServiceCollection AddRandomSerializerTestApp(this IServiceCollection services)
    {
        services
            .AddCore(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
            .AddScoped<Startup>();

        return services;
    }
}