using System.ComponentModel;

namespace Inmeta.Test.Data.Models.Enums
{
    [Flags]
    public enum OrderStatus : byte
    {
        None = 0,

        [Description("The order is pending.")]
        Pending = 1,

        [Description("The order is being processed.")]
        Processing = 2,

        [Description("The order has been shipped or delivered.")]
        Shipped = 4,

        [Description("The order has been delivered to the customer.")]
        Delivered = 8,

        [Description("The order has been cancelled.")]
        Cancelled = 16,

        [Description("The payment for the order has been refunded.")]
        Refunded = 32,

        [Description("The items from the order have been returned.")]
        Returned = 64
    }
}