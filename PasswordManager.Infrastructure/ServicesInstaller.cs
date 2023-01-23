using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.Interfaces;
using PasswordManager.Infrastructure.Services;
using PasswordManager.Infrastructure.ServiceSettings;
using PasswordManager.Infrastructure.ToolServices;
using PasswordManager.Shared;

namespace PasswordManager.Infrastructure;

public static class ServicesInstaller
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterSqlDatabase(configuration.GetConnectionString("publicdb")!);
        services.RegisterBulkStorage(configuration.GetConnectionString("bulkstorage")!);

        return services;
    }

    public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ImageManipulationService>();

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IPasswordCategoriesService, PasswordCategoryService>();

        services.AddScopedWithSettings<IImagesService, ImagesService, ImagesServiceSettings>(configuration);
        services.AddScopedWithSettings<IAuthorizationService, AuthorizationService, AuthorizationServiceSettings>(configuration);

        return services;
    }
}
