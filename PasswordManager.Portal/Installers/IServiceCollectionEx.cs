using PasswordManager.Portal.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            .AddScoped<PasswordLogoService>()
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
            WriteIndented = true,
            DefaultBufferSize = 128
        };
        serializerOptions.Converters.Add(new JsonStringEnumConverter());

        return services.AddSingleton(s => serializerOptions);
    }
}
