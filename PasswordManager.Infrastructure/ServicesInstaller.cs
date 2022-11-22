using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure;

public static class ServicesInstaller
{
    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        services.AddSingleton<MDbClient>();
        services.AddSingleton<IUsersService, UsersService>();

        return services;
    }
}
