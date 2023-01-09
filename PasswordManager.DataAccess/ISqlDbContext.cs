using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DataAccess.DbModels;

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
        services.AddDbContext<ISqlDbContext, SqlDatabaseContext>(builder =>
        {
            builder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure(5);
            });
        });

        return services;
    }
}