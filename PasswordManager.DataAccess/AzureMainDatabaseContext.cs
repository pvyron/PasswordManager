using Microsoft.EntityFrameworkCore;
using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess;

public class AzureMainDatabaseContext : DbContext
{

    public DbSet<UserDbModel> Users { get; set; } = default!;
    public DbSet<PasswordDbModel> Passwords { get; set; } = default!;
    public DbSet<PasswordCategoryDbModel> PasswordCategories { get; set; } = default!;

    public AzureMainDatabaseContext(DbContextOptions<AzureMainDatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
