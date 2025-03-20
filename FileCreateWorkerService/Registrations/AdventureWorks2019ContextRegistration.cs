using FileCreateWorkerService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileCreateWorkerService.Registrations;

public static class AdventureWorks2019ContextRegistration
{
    public static void AddAdventureWorks2019ContextRegistration(this IServiceCollection services,IConfiguration config)
    {
        services.AddDbContext<AdventureWorks2019Context>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("SqlServer"));
        });
    }
}
