using Microsoft.AspNetCore.Identity;
using RabbitMQWeb.ExcelCreate.Models;
using RabbitMQWeb.ExcelCreate.Registrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAppDbContextRegistration(builder.Configuration);
builder.Services.AddRabbitMQRegistration(builder.Configuration);
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string adminRole = "Admin";
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    string adminEmail = "ZMadmin@gmail.com";
    string adminPassword = "Salam123@"; 

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new AppUser
        {
            UserName = "zmadmin_040",
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
    string userRole = "Member";
    if (!await roleManager.RoleExistsAsync(userRole))
    {
        await roleManager.CreateAsync(new IdentityRole(userRole));
    }

    string userEmail = "ZM@gmail.com";
    string userPassword = "Salam123@";

    if (await userManager.FindByEmailAsync(userEmail) == null)
    {
        var User = new AppUser
        {
            UserName = "zm_040",
            Email = userEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(User, userPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(User, userRole);
        }
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
