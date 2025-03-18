using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.ExcelCreate.Contexts;
using RabbitMQWeb.ExcelCreate.Models;

namespace RabbitMQWeb.ExcelCreate.Registrations;

public static class AppDbContextRegistration
{
    public static void AddAppDbContextRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("default"));
        }).AddIdentity<AppUser, IdentityRole>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequiredLength = 8;
            opt.Password.RequireDigit = true;
            opt.User.RequireUniqueEmail = true;
            opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789._";
        })
        .AddEntityFrameworkStores<AppDbContext>(); ;
    }
}
