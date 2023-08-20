using Inmeta.Test.Data.Models.Enums;

namespace Inmeta.Test.Data.Abstractions.Entities
{
	public interface IAddress
    {
		string StreetAddress { get; }
		ZipCode ZipCode { get; }
		string? City { get; }
		Country? Country { get; }
	}
}
