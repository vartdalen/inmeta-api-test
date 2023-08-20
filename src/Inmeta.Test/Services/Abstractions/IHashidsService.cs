using HashidsNet;

namespace Inmeta.Test.Services.Abstractions
{
    public interface IHashidsService
    {
        IHashids Get(Type entityType);
    }
}