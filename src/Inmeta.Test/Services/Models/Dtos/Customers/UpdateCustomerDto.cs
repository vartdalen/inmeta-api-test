using Inmeta.Test.Services.Abstractions.Nullable;
using System.ComponentModel.DataAnnotations;

namespace Inmeta.Test.Services.Models.Dtos.Customers
{
	public readonly record struct UpdateCustomerDto : INullableCustomer
    {
        [MaxLength(255)]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
        public string? Email { get; init; }
		[MaxLength(15)]
		[RegularExpression(@"^\+\d{1,}[\d\s-]{5,}$")]
		public string? Phone { get; init; }
		[MaxLength(255)]
		[MinLength(2)]
		[RegularExpression(@"^[^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$")]
		public string? Name { get; init; }
	}
}
