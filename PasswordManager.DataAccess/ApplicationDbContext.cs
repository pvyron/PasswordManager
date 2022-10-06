using Microsoft.EntityFrameworkCore;
using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess
{
    internal sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<UserDbModel> Users { get; set; } = null!;
        public DbSet<PasswordDbModel> Passwords { get; set; } = null!;
    }
}