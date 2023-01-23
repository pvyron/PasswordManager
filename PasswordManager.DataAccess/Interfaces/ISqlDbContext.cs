using Microsoft.EntityFrameworkCore;
using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess.Interfaces;

public interface ISqlDbContext
{
    DbSet<UserDbModel> Users { get; set; }
    DbSet<PasswordDbModel> Passwords { get; set; }
    DbSet<PasswordCategoryDbModel> PasswordCategories { get; set; }
    DbSet<PasswordLogoDbModel> PasswordLogos { get; set; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}