using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recarro.Data;
using Recarro.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

using static Recarro.WebConstants;

namespace Recarro.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var provider = scope.ServiceProvider;

            var data = scope.ServiceProvider.GetRequiredService<RecarroDbContext>();

            data.Database.Migrate();

            SeedCategories(data);
            SeedEngineTypes(data);
            SeedAdministrator(provider);

            data.SaveChanges();

            return app;
        }

        private static void SeedEngineTypes(RecarroDbContext data)
        {
            if (data.EngineTypes.Any())
            {
                return;
            }

            data.EngineTypes.AddRange(new[]
            {
                new EngineType { Type = "Gasoline" },
                new EngineType { Type = "Diesel" },
                new EngineType { Type = "Hybrid" },
                new EngineType { Type = "Electric" }
            });
        }

        private static void SeedCategories(RecarroDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Hatchback" },
                new Category { Name = "Sedan" },
                new Category { Name = "Coupe" },
                new Category { Name = "SUV" },
                new Category { Name = "Offroad" },
                new Category { Name = "Pickup" },
                new Category { Name = "Wagon" },
                new Category { Name = "Van" },
                new Category { Name = "Sport" },
                new Category { Name = "Luxury" },
                new Category { Name = "Muscle" },
                new Category { Name = "Cabriolet" }
            });
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                {
                    return;
                }

                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = AdministratorRoleName
                });

                var user = new IdentityUser
                {
                    Email = AdminEmail,
                    UserName = AdminUsername
                };

                await userManager.CreateAsync(user, AdminPassword);

                await userManager.AddToRoleAsync(user, AdministratorRoleName);
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
