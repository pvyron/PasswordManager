using Microsoft.EntityFrameworkCore;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.DataAccess.Interfaces;

namespace PasswordManager.DataAccess.Implementations;

internal class SqlDatabaseContext : DbContext, ISqlDbContext
{

    public DbSet<UserDbModel> Users { get; set; } = default!;
    public DbSet<PasswordDbModel> Passwords { get; set; } = default!;
    public DbSet<PasswordCategoryDbModel> PasswordCategories { get; set; } = default!;

    public SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
