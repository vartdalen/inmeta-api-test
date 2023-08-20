using Inmeta.Test.Data.Abstractions;
using Inmeta.Test.Data.Abstractions.Entities;
using Inmeta.Test.Data.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmeta.Test.Data.Models.Entities
{
	[Table("addresses")]
	public class Address : IAddress, IIdentifiable, IAuditable
	{
		[Column("id")]
		public int Id { get; set; }
		[Column("created_at")]
		public DateTimeOffset CreatedAt { get; set; }
		[Column("modified_at")]
		public DateTimeOffset ModifiedAt { get; set; }
		[Column("street_address")]
		public string StreetAddress { get; set; }
		[Column("zip_code")]
		public ZipCode ZipCode { get; set; }
		[Column("city")]
		public string? City { get; set; }
		[Column("country")]
		public Country? Country { get; set; }

		public virtual ICollection<Order> FromOrders { get; set; }
		public virtual ICollection<Order> ToOrders { get; set; }
	}

	[Table("customers")]
    public class Customer : ICustomer, IIdentifiable, IAuditable
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTimeOffset ModifiedAt { get; set; }
        [Column("email")]
        public string Email { get; set; }
		[Column("phone")]
		public string? Phone { get; set; }
		[Column("name")]
		public string Name { get; set; }

		public virtual ICollection<Order> Orders { get; set; }
	}

	[Table("orders")]
    public class Order : IOrder, IIdentifiable, IAuditable
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTimeOffset ModifiedAt { get; set; }
		[Column("service_at")]
		public DateTimeOffset ServiceAt { get; set; }
		[Column("order_status")]
        public OrderStatus OrderStatus { get; set; }
		[Column("service")]
		public Service Service { get; set; }

		public virtual ICollection<OrderLog> OrderLogs { get; set; }

		[Column("customer_id")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
		[Column("from_address_id")]
		public int FromAddressId { get; set; }
		public virtual Address FromAddress { get; set; }
		[Column("to_address_id")]
		public int ToAddressId { get; set; }
		public virtual Address ToAddress { get; set; }
	}

	[Table("order_logs")]
	public class OrderLog : IIdentifiable, IAuditable, ILoggable
	{
		[Column("id")]
		public int Id { get; set; }
		[Column("created_at")]
		public DateTimeOffset CreatedAt { get; set; }
		[Column("modified_at")]
		public DateTimeOffset ModifiedAt { get; set; }
		[Column("message")]
		public string Message { get; set; }

		[Column("order_id")]
		public int OrderId { get; set; }
		public virtual Order Order { get; set; }
	}
}