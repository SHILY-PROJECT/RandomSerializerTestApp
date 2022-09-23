using Microsoft.Extensions.DependencyInjection;
using RandomSerializerTestApp.Core.LocalStorage;
using RandomSerializerTestApp.Core.Persons;

namespace RandomSerializerTestApp.Core.Configuration;

public static class CoreRegistration
{
    public static IServiceCollection AddCore(this IServiceCollection services, string directoryNameOfFiles)
    {
        services
            .AddScoped<ILocalStorageServiceSettings>(x => new LocalStorageServiceSettings { DirectoryNameOfFiles = directoryNameOfFiles })
            .AddScoped<ILocalStorageService, LocalStorageService>()
            .AddScoped<IPersonGeneratorServiceSettings, PersonGeneratorServiceSettings>()
            .AddScoped<IPersonGeneratorService, PersonGeneratorService>();

        return services;
    }
}