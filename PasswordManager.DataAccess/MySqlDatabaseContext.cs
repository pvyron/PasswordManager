using Microsoft.EntityFrameworkCore;
using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess;

internal class MySqlDatabaseContext : DbContext, ISqlDbContext
{

    public DbSet<UserDbModel> Users { get; set; } = default!;
    public DbSet<PasswordDbModel> Passwords { get; set; } = default!;
    public DbSet<PasswordCategoryDbModel> PasswordCategories { get; set; } = default!;

    public MySqlDatabaseContext(DbContextOptions<MySqlDatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
