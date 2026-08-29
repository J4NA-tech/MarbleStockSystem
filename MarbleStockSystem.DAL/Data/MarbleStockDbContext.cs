using Microsoft.EntityFrameworkCore;
using MarbleStockSystem.DAL.Entities;

namespace MarbleStockSystem.DAL.Data
{
    /// <summary>
    /// Entity Framework Core DbContext sınıfı
    /// Veritabanı bağlantısı ve entity yapılandırmalarını yönetir
    /// </summary>
    public class MarbleStockDbContext : DbContext
    {
        /// <summary>
        /// Constructor - DbContextOptions ile yapılandırma alır
        /// </summary>
        public MarbleStockDbContext(DbContextOptions<MarbleStockDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Mermerler tablosu
        /// </summary>
        public DbSet<Marble> Marbles { get; set; }

        /// <summary>
        /// Müşteriler tablosu
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Satışlar tablosu
        /// </summary>
        public DbSet<Sale> Sales { get; set; }

        /// <summary>
        /// Model yapılandırması - Fluent API ile ilişkiler ve kısıtlamalar tanımlanır
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Marble entity yapılandırması
            modelBuilder.Entity<Marble>(entity =>
            {
                entity.HasKey(e => e.MarbleId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Thickness).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PricePerM2).HasColumnType("decimal(18,2)");
                entity.Property(e => e.StockQuantity).HasColumnType("decimal(18,2)");

                // Marble ile Sale arasındaki ilişki
                entity.HasMany(e => e.Sales)
                      .WithOne(e => e.Marble)
                      .HasForeignKey(e => e.MarbleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Customer entity yapılandırması
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);

                // Customer ile Sale arasındaki ilişki
                entity.HasMany(e => e.Sales)
                      .WithOne(e => e.Customer)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Sale entity yapılandırması
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.SaleId);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SaleDate).IsRequired();

                // Index'ler performans için
                entity.HasIndex(e => e.MarbleId);
                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.SaleDate);
            });
        }
    }
}



