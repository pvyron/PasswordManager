using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DataAccess;

namespace PasswordManager.Application
{
    public static class Extensions
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
            return services;
        }
    }
}
