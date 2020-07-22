using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactocommerce.Models;

namespace Transactocommerce.Utilities
{
    public class DataContext : DbContext
    {
        private IConfiguration _configuration;
        public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(_configuration.GetSection("ConnectionString").Value);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product table
            // Foreign key relationship 1 - 1 (product-category)
            //modelBuilder.Entity<Product>()
              //  .HasOne(p => p.)


            // Category table
            // Unique category
            modelBuilder.Entity<Category>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
