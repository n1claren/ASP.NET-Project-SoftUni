using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recarro.Data.Models;

namespace Recarro.Data
{
    public class RecarroDbContext : IdentityDbContext
    {
        public RecarroDbContext(DbContextOptions<RecarroDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<EngineType> EngineTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Vehicle>()
                .HasOne(v => v.Category)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Vehicle>()
                .HasOne(v => v.EngineType)
                .WithMany(et => et.Vehicles)
                .HasForeignKey(v => v.EngineTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Vehicle>()
                .Property(p => p.PricePerDay)
                .HasColumnType("decimal(18,4)");

            base.OnModelCreating(builder);
        }
    }
}
