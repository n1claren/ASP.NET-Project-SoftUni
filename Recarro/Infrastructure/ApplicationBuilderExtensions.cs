using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recarro.Data;
using Recarro.Data.Models;
using System;
using System.Linq;

namespace Recarro.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var data = scope.ServiceProvider.GetService<RecarroDbContext>();

            data.Database.Migrate();

            SeedCategories(data);
            SeedEngineTypes(data);

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
    }
}
