namespace Inmeta.Test.Data.Abstractions.Repositories
{
    public interface ICrudRepository<TId, TEntity, TRequest>
    {
        Task<TEntity> Create(TRequest request);
        Task<TEntity?> Read(TId id);
        Task Update(TRequest request);
        Task Delete(TId id);
    }
}
