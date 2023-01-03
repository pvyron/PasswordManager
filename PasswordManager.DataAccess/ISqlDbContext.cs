using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess;

public interface ISqlDbContext
{
    DbSet<UserDbModel> Users { get; set; }
    DbSet<PasswordDbModel> Passwords { get; set; }
    DbSet<PasswordCategoryDbModel> PasswordCategories { get; set; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public static class IServiceCollectionEx
{
    public static IServiceCollection RegisterSqlDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ISqlDbContext, MySqlDatabaseContext>(builder =>
        {
            builder.UseMySql(connectionString, new MySqlServerVersion(new Version()), options =>
            {
                options.EnableRetryOnFailure();
            });
        });

        return services;
    }
}