using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordManager.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
