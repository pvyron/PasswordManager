using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.Infrastructure.Services;
using PasswordManager.Infrastructure.ServiceSettings;
using PasswordManager.Shared;

namespace PasswordManager.Infrastructure;

public static class ServicesInstaller
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<MDbClient>();
        //services.RegisterSqlDatabase(configuration.GetConnectionString("MainSqlDatabase")!);
        services.RegisterSqlDatabase("");

        return services;
    }

    public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IPasswordCategoriesService, PasswordCategoryService>();

        services.AddScopedWithSettings<IAuthorizationService, AuthorizationService, AuthorizationServiceSettings>(configuration);

        return services;
    }
}
