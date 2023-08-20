using Inmeta.Test.Data.Models.Entities;

namespace Inmeta.Test.Data.Models.Requests
{
	public readonly record struct AddressRequest(Address Address);
	public readonly record struct CustomerRequest(Customer Customer);
	public readonly record struct OrderRequest(Order Order);
	public readonly record struct OrderLogRequest(OrderLog OrderLog);
}
