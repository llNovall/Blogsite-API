using BlogsiteAppAccountAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace BlogsiteAPI.Utils
{
    public static class IApplicationBuilderExtension
    {
        public static void EnsureIdentityDbCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                AccountDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AccountDbContext>();

                dbContext.Database.Migrate();
            }
        }
    }
}