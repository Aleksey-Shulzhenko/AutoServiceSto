using Microsoft.EntityFrameworkCore;
using AutoServiceSto.Models;

namespace AutoServiceSto.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<FinanceRecord> FinanceRecords { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<MaintenancePartRecord> MaintenancePartRecords { get; set; }

        // Конструктор без параметров
        public AppDbContext() { }

        // Конструктор с параметрами для Dependency Injection
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=autoservice.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Базовая конфигурация моделей
        }
    }
}