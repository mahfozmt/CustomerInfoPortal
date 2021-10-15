using CustomerInfoPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfoPortal
{
    public class CIPDbContext:DbContext
    {
        private readonly DbContextOptions _options;
        public CIPDbContext(DbContextOptions options) : base(options) {_options = options; }

        public DbSet<Country> Country { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country {ID = 1, CountryName = "Bangladesh"},
                new Country {ID = 2, CountryName = "India" },
                new Country {ID = 3, CountryName = "Nepal" }
            );
        }

    }
}
