using Microsoft.EntityFrameworkCore;
using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Persistence.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<VillaImage> VillaImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
        new Villa
        {
            Id = 1,
            Name = "Royal Villa",
            Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
            Occupancy = 4,
            Price = 200,
            Sqft = 550,
        },
        new Villa
        {
            Id = 2,
            Name = "Premium Pool Villa",
            Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
            Occupancy = 4,
            Price = 300,
            Sqft = 550,
        },
        new Villa
        {
            Id = 3,
            Name = "Luxury Pool Villa",
            Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
            Occupancy = 4,
            Price = 400,
            Sqft = 750,
        });
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 101,
                    VillaId = 1,
                },
                 new VillaNumber
                 {
                     Villa_Number = 102,
                     VillaId = 1,
                 },
                  new VillaNumber
                  {
                      Villa_Number = 103,
                      VillaId = 1,
                  },
                   new VillaNumber
                   {
                       Villa_Number = 104,
                       VillaId = 1,
                   },
                    new VillaNumber
                    {
                        Villa_Number = 201,
                        VillaId = 2,
                    },
                     new VillaNumber
                     {
                         Villa_Number = 202,
                         VillaId = 2,
                     },
                      new VillaNumber
                      {
                          Villa_Number = 203,
                          VillaId = 2,
                      });
        }
    }
}
