using System.Linq.Expressions;

using EfLight.Utils;

namespace EfLight.Abstractions;
public interface IPagingAndSortingRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    IEnumerable<TEntity> FindAll();


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// </summary>
    IEnumerable<TEntity> FindAll(PageRequest page);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    IEnumerable<TEntity> FindAll(PageRequest page, Expression<Func<TEntity, bool>> predicateFn);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    IEnumerable<TEntity> FindAll<TOrderKey>(PageRequest page, Expression<Func<TEntity, TOrderKey>> orderKey);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    IEnumerable<TEntity> FindAll<TOrderKey>(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, Expression<Func<TEntity, TOrderKey>> orderKey);


    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    Task<IEnumerable<TEntity>> FindAllAsync(PageRequest page, CancellationToken cancellationToken = default);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> fullfilling
    /// the given <paramref name="predicateFn"/> condition.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    Task<IEnumerable<TEntity>> FindAllAsync(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(PageRequest page, Expression<Func<TEntity, TOrderKey>> orderKey, CancellationToken cancellationToken = default);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, Expression<Func<TEntity, TOrderKey>> orderKey, CancellationToken cancellationToken = default);
}

