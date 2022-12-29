using System.Text.Json.Serialization;
using System.Text.Json;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Installers;

public static class IServiceCollectionEx
{
    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ClientStateData>()
            .AddScoped<ApiClient>()
            .AddScoped<AuthenticationService>()
            .AddScoped<PasswordsService>()
            .AddScoped<CategoriesService>()
            .AddSingleton<ClipboardService>();
    }

    public static IServiceCollection AddJsonOptions(this IServiceCollection services)
    {
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            MaxDepth = 10,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };
        serializerOptions.Converters.Add(new JsonStringEnumConverter());

        return services.AddSingleton(s => serializerOptions);
    }
}
