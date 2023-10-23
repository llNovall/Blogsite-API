using BlogsiteAppAccountAccess.Context;
using BlogsiteDomain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogsiteAPI.Utils
{
    public static class IApplicationBuilderExtension
    {
        public static void CreateAdminUser(this IApplicationBuilder builder)
        {
            IConfiguration? config = builder.ApplicationServices.GetService<IConfiguration>() ?? throw new NullReferenceException(nameof(IConfiguration));

            IWebHostEnvironment? environment = builder.ApplicationServices.GetService<IWebHostEnvironment>() ?? throw new NullReferenceException(nameof(IWebHostEnvironment));

            string adminUserName = "";
            string adminPassword = "";
            string adminEmail = "";
            string adminRole = "";

            if (environment.IsEnvironment("Development"))
            {
                adminUserName = config.GetSection("admin").GetRequiredSection("Username").Value ?? throw new NullReferenceException(nameof(adminUserName));
                adminPassword = config.GetSection("admin").GetRequiredSection("Password").Value ?? throw new NullReferenceException(nameof(adminPassword));
                adminEmail = config.GetSection("admin").GetRequiredSection("Email").Value ?? throw new NullReferenceException(nameof(adminEmail));
                adminRole = config.GetSection("admin").GetRequiredSection("Role").Value ?? throw new NullReferenceException(nameof(adminRole));
            }
            else
            {
                adminUserName = config.GetSection("adminUsername").Get<string>() ?? throw new NullReferenceException(nameof(adminUserName));
                adminPassword = config.GetSection("adminPassword").Get<string>() ?? throw new NullReferenceException(nameof(adminPassword));
                adminEmail = config.GetSection("adminEmail").Get<string>() ?? throw new NullReferenceException(nameof(adminEmail));
                adminRole = config.GetSection("adminRole").Get<string>() ?? throw new NullReferenceException(nameof(adminRole));
            }

            if (string.IsNullOrEmpty(adminUserName) || string.IsNullOrEmpty(adminPassword) || string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminRole))
                throw new NullReferenceException("Admin data is missing to create admin account.");

            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                UserManager<AppUser>? userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (userManager == null)
                    throw new NullReferenceException(nameof(UserManager<AppUser>));

                AppUser? admin = userManager.FindByNameAsync(adminUserName).GetAwaiter().GetResult();

                if (admin != null)
                    return;

                admin = new AppUser(adminUserName);
                userManager.SetEmailAsync(admin, adminEmail).GetAwaiter().GetResult();
                var result = userManager.CreateAsync(admin, adminPassword).GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    userManager.AddClaimAsync(admin, new Claim(ClaimTypes.Role, adminRole)).GetAwaiter().GetResult();
                }
            }

            return;
        }

        public static void EnsureIdentityDbCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                AccountDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AccountDbContext>();

                if (dbContext.Database.EnsureCreated())
                    dbContext.Database.Migrate();
            }
        }
    }
}