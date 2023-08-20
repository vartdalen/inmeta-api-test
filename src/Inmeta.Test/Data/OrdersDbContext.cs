using Inmeta.Test.Data.Extensions;
using Inmeta.Test.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inmeta.Test.Data
{
	public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }

		public DbSet<Address> Addresses { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderLog> OrderLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureProperties(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Address>()
				.HasMany(a => a.FromOrders)
				.WithOne(o => o.FromAddress)
				.HasForeignKey(o => o.FromAddressId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Address>()
				.HasMany(a => a.ToOrders)
				.WithOne(o => o.ToAddress)
				.HasForeignKey(o => o.ToAddressId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<OrderLog>()
				.HasOne(ol => ol.Order)
				.WithMany(o => o.OrderLogs)
				.HasForeignKey(ol => ol.OrderId)
				.OnDelete(DeleteBehavior.Restrict);
		}

        private void ConfigureProperties(ModelBuilder modelBuilder)
        {
			modelBuilder.ConfigureProperties<Address>();
			modelBuilder.ConfigureProperties<Customer>();
			modelBuilder.ConfigureProperties<Order>();
			modelBuilder.ConfigureProperties<OrderLog>();

			modelBuilder.Entity<Address>()
				.Property(a => a.ZipCode)
				.HasConversion<int>();

			modelBuilder.Entity<Address>()
				.Property(a => a.Country)
				.HasConversion<int>();

			modelBuilder.Entity<Address>()
				.Property(a => a.StreetAddress)
				.HasMaxLength(255);

			modelBuilder.Entity<Address>()
				.Property(a => a.City)
				.HasMaxLength(255);

			modelBuilder.Entity<Address>()
				.HasIndex(a => new { a.StreetAddress, a.ZipCode, a.City, a.Country })
				.IsUnique();

			modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(255);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

			modelBuilder.Entity<Customer>()
				.Property(c => c.Phone)
				.HasMaxLength(15);

			modelBuilder.Entity<Customer>()
				.Property(c => c.Name)
				.HasMaxLength(255);

			modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<byte>();

			modelBuilder.Entity<Order>()
				.Property(o => o.Service)
				.HasConversion<byte>();

			modelBuilder.Entity<OrderLog>()
				.Property(ol => ol.Message)
				.HasMaxLength(255);
		}
    }
}