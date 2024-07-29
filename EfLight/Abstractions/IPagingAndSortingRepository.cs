using System.Linq.Expressions;
using EfLight.Common;

namespace EfLight.Abstractions;

public interface IPagingAndSortingRepository<TEntity, in TKey> : ICrudRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Retrieves all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    IEnumerable<TEntity> FindAll(bool track = false);


    /// <summary>
    ///     Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    ///     <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    ///     of <typeparamref name="TEntity"/>.
    /// </summary>
    IEnumerable<TEntity> FindAll(PaginationRequest page, bool track = false);


    /// <summary>
    /// Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    /// <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    /// of <typeparamref name="TEntity"/> that matches the provided <paramref name="predicate"/>.
    /// </summary>
    IEnumerable<TEntity> FindAll(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicate,
        bool track = false
    );


    /// <summary>
    /// Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    /// <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    /// of <typeparamref name="TEntity"/> ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    IEnumerable<TEntity> FindAll<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false
    );


    /// <summary>
    /// Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    /// <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    /// of <typeparamref name="TEntity"/> that matches the provided <paramref name="predicate"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="page"></param>
    IEnumerable<TEntity> FindAll<TOrderKey>(PaginationRequest page,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false
    );


    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync(bool track = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    /// <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync(
        PaginationRequest page,
        bool track = false,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    /// Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    /// <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    /// of <typeparamref name="TEntity"/> that matches the provided <paramref name="predicate"/>.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync(
        PaginationRequest page, Expression<Func<TEntity, bool>> predicate,
        bool track = false,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    /// Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    /// <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    /// of <typeparamref name="TEntity"/> ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default
    );


    /// <summary>
    /// Returns a given set of at most (<see cref="P:EfLight.Common.PaginationRequest.Index"/> *
    /// <see cref="P:EfLight.Common.PaginationRequest.Offset"/>) + <see cref="P:EfLight.Common.PaginationRequest.Offset"/>
    /// of <typeparamref name="TEntity"/> that matches the provided <paramref name="predicate"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default
    );
}