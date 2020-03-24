using CarDealershipApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarDealershipApp.DAL.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                 new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                 new { Id = "2", Name = "SalesRep", NormalizedName = "SALESREP" }
                );
            builder.Entity<Country>().HasData(
                 new { Id=Guid.NewGuid(), Name = "South Africa", Code = "ZA" }
                );
            builder.Entity<ProductType>().HasData(
                 new { Id = 1, Description = "Sedan" },
                 new { Id = 2, Description = "Hatchback" },
                 new { Id = 3, Description = "Bakkie" }
                );
            builder
            .Entity<Order>()
            .HasOne<ClientProfile>(e => e.Profile)
            .WithMany(e => e.Orders)
            .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProfilePicture> ProfilePicture { get; set; }
    }
}
