using Microsoft.EntityFrameworkCore;
using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess;

internal class SqlDatabaseContext : DbContext, ISqlDbContext
{

    public DbSet<UserDbModel> Users { get; set; } = default!;
    public DbSet<PasswordDbModel> Passwords { get; set; } = default!;
    public DbSet<PasswordCategoryDbModel> PasswordCategories { get; set; } = default!;

    public SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
