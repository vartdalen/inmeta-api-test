using Inmeta.Test.Data.Abstractions.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inmeta.Test.Services.Models.Dtos.Customers
{
	public readonly record struct CreateCustomerDto : ICustomer
    {
        [JsonRequired]
		[MaxLength(255)]
		[RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
		public string Email { get; init; }
		[MaxLength(15)]
		[RegularExpression(@"^\+\d{1,}[\d\s-]{5,}$")]
		public string Phone { get; init; }
		[JsonRequired]
		[MaxLength(255)]
		[MinLength(2)]
		[RegularExpression(@"^[^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$")]
		public string Name { get; init; }
	}
}
