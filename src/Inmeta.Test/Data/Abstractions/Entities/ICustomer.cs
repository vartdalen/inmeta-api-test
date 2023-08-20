namespace Inmeta.Test.Data.Abstractions.Entities
{
    public interface ICustomer
    {
        string Email { get; }
		string? Phone { get; }
		string Name { get; }
	}
}
