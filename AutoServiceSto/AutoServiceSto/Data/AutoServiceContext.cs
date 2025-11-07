using Microsoft.EntityFrameworkCore;
using AutoServiceSto.Models;

namespace AutoServiceSto.Data
{
    public class AutoServiceContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<FinanceRecord> FinanceRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=autoservice.db");
        }
    }
}
