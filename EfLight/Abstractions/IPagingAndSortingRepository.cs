using System.Linq.Expressions;

using EfLight.Utils;

namespace EfLight.Abstractions;
internal interface IPagingAndSortingRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey>
    where TEntity : class
{
    IEnumerable<TEntity> FindAll();

    IEnumerable<TEntity> FindAll(PageRequest page);

    IEnumerable<TEntity> FindAll(PageRequest page, Expression<Func<TEntity, bool>> predicateFn);

    IEnumerable<TEntity> FindAll<TOrderKey>(PageRequest page, Expression<Func<TEntity, TOrderKey>> orderKey);

    IEnumerable<TEntity> FindAll<TOrderKey>(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, Expression<Func<TEntity, TOrderKey>> orderKey);

    Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAllAsync(PageRequest page, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAllAsync(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(PageRequest page, Expression<Func<TEntity, TOrderKey>> orderKey, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, Expression<Func<TEntity, TOrderKey>> orderKey, CancellationToken cancellationToken = default);
}

