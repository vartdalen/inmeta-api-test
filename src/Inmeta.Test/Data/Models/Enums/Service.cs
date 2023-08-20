namespace Inmeta.Test.Data.Models.Enums
{
	[Flags]
	public enum Service : byte
	{
		None = 0,
		Moving = 1,
		Packing = 2,
		Cleaning = 4
	}
}
