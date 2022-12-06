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

namespace PasswordManager.Infrastructure;

public static class ServicesInstaller
{
    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        services.AddScoped<MDbClient>();

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IPasswordCategoriesService, PasswordCategoryService>();

        services.AddScoped<IAuthorizationService, AuthorizationService>();

        return services;
    }
}
