using System.Linq.Expressions;
using EfLight.Common;

namespace EfLight.Abstractions;

public interface IPagingAndSortingRepository<TEntity, TKey> : ICrudRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    IEnumerable<TEntity> FindAll(bool track = false);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// </summary>
    IEnumerable<TEntity> FindAll(PaginationRequest page, bool track = false);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    IEnumerable<TEntity> FindAll(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicateFn,
        bool track = false
    );


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    IEnumerable<TEntity> FindAll<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false
    );


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    IEnumerable<TEntity> FindAll<TOrderKey>(PaginationRequest page,
        Expression<Func<TEntity, bool>> predicateFn,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false
    );


    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync(bool track = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a given set of at most <see cref="page.offset"/> of <typeparamref name="TEntity"/> fullfilling
    /// the given <paramref name="predicateFn"/> condition.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync(
        PaginationRequest page,
        bool track = false,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    /// Returns a given set of at most <see cref="page.offset"/> of <typeparamref name="TEntity"/> fullfilling
    /// the given <paramref name="predicateFn"/> condition.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    Task<IEnumerable<TEntity>> FindAllAsync(
        PaginationRequest page, Expression<Func<TEntity, bool>> predicateFn,
        bool track = false,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicateFn,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default
    );
}