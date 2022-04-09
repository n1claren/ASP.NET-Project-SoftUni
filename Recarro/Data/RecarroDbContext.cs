using Microsoft.AspNetCore.Identity;
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

        public DbSet<Vehicle> Vehicles { get; init; }

        public DbSet<Category> Categories { get; init; }

        public DbSet<EngineType> EngineTypes { get; init; }

        public DbSet<Renter> Renters { get; init; }

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

            builder
                .Entity<Vehicle>()
                .HasOne(v => v.Renter)
                .WithMany(r => r.Vehicles)
                .HasForeignKey(v => v.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Renter>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Renter>(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Rent>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Rent>(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Rent>()
                .HasOne<Vehicle>()
                .WithMany(v => v.Rents)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Rent>()
                .Property(r => r.Bill)
                .HasColumnType("decimal(18,4)");

            base.OnModelCreating(builder);
        }
    }
}
