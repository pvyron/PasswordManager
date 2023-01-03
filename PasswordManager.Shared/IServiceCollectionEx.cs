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
        services.Configure<TSettings>(configuration.GetSection(typeof(TImplementation).Name));

        return services.AddSingleton<TService, TImplementation>();
    }

    public static IServiceCollection AddScopedWithSettings
        <TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, TSettings>
        (this IServiceCollection services, IConfiguration configuration)
        where TService : class
        where TImplementation : class, TService
        where TSettings : class, IServiceSettings
    {
        services.Configure<TSettings>(configuration.GetSection(typeof(TImplementation).Name));

        return services.AddScoped<TService, TImplementation>();
    }

    public static IServiceCollection AddTransientWithSettings
        <TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, TSettings>
        (this IServiceCollection services, IConfiguration configuration)
        where TService : class
        where TImplementation : class, TService
        where TSettings : class, IServiceSettings
    {
        services.Configure<TSettings>(configuration.GetSection(typeof(TImplementation).Name));

        return services.AddTransient<TService, TImplementation>();
    }
}
