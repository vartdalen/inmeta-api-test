using Inmeta.Test.Data.Models.Enums;

namespace Inmeta.Test.Services.Abstractions.Nullable
{
	public interface INullableAddress
    {
		string? StreetAddress { get; }
		ZipCode? ZipCode { get; }
		string? City { get; }
		Country? Country { get; }
	}
}
