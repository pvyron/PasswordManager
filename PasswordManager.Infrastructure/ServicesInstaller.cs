using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Infrastructure;

public static class ServicesInstaller
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<MDbClient>();
        services.AddDbContext<AzureMainDatabaseContext>(builder =>
        {
            builder.UseMySql(configuration.GetConnectionString("MainDatabase")!, new MySqlServerVersion(new Version()), options =>
            {
                options.EnableRetryOnFailure();
            });
        });

        return services;
    }

    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IPasswordCategoriesService, PasswordCategoryService>();

        services.AddScoped<IAuthorizationService, AuthorizationService>();

        return services;
    }
}
