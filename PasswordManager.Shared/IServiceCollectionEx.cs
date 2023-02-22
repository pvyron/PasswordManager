using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Shared.Models;
using System.Diagnostics.CodeAnalysis;

namespace PasswordManager.Shared;

public static class IServiceCollectionEx
{
    public static IServiceCollection AddSingletonWithSettings
        <TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, TSettings>
        (this IServiceCollection services, IConfiguration configuration)
        where TService : class
        where TImplementation : class, TService
        where TSettings : class, IServiceSettings
    {
        services.AddOptions<TSettings, TImplementation>(configuration);

        return services.AddSingleton<TService, TImplementation>();
    }

    public static IServiceCollection AddScopedWithSettings
        <TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, TSettings>
        (this IServiceCollection services, IConfiguration configuration)
        where TService : class
        where TImplementation : class, TService
        where TSettings : class, IServiceSettings
    {
        services.AddOptions<TSettings, TImplementation>(configuration);

        return services.AddScoped<TService, TImplementation>();
    }

    public static IServiceCollection AddTransientWithSettings
        <TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, TSettings>
        (this IServiceCollection services, IConfiguration configuration)
        where TService : class
        where TImplementation : class, TService
        where TSettings : class, IServiceSettings
    {
        services.AddOptions<TSettings, TImplementation>(configuration);

        return services.AddTransient<TService, TImplementation>();
    }

    private static void AddOptions<TSettings, TService>(this IServiceCollection services, IConfiguration configuration)
        where TService : class
        where TSettings: class, IServiceSettings
    {
        services.AddOptions<TSettings>()
            .Bind(configuration.GetRequiredSection(typeof(TService).Name))
            .Validate(x => x.Validate(), $"Settings validation failed for {typeof(TService).Name}.")
            .ValidateOnStart();
    }
}
