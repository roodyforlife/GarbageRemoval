using GarbageRemoval.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.DataBase
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Administration> Administrations { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Brigade> Brigades { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=DESKTOP-KIV92L3;Database=GarbadgeRemoval;Trusted_Connection=True;Encrypt=False;");
            // optionsBuilder.UseSqlServer("Server=DESKTOP-I75L3P7;Database=GarbadgeRemoval;Trusted_Connection=True;Encrypt=False;");
             optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GarbadgeRemoval;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>().HasMany(x => x.Orders).WithOne(x => x.House).HasForeignKey(x => x.HouseId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
