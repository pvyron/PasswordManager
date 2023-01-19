using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DataAccess.Implementations;
using PasswordManager.DataAccess.Interfaces;

namespace PasswordManager.DataAccess;

public static class DataAccessInstaller
{
    public static IServiceCollection RegisterSqlDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ISqlDbContext, SqlDatabaseContext>(builder =>
        {
            builder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure(5);
            });
        });

        return services;
    }

    public static IServiceCollection RegisterBulkStorage(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IBulkStorageService>(new BulkStorageService(connectionString));

        return services;
    }
}
