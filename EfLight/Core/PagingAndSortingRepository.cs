using System.Linq.Expressions;
using EfLight.Abstractions;
using EfLight.Common;
using Microsoft.EntityFrameworkCore;

namespace EfLight.Core;

public abstract class PagingAndSortingRepository<TEntity, TKey>(DbContext context)
    : CrudRepository<TEntity, TKey>(context),
        IPagingAndSortingRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    ///     Retrives all records held in <typeparamref name="TEntity" /> entity's table.
    /// </summary>
    public IEnumerable<TEntity> FindAll(bool track = false)
    {
        return track switch
        {
            true => _context.Set<TEntity>().ToList(),
            false => _context.Set<TEntity>().AsNoTrackingWithIdentityResolution().ToList()
        };
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="PageSize" /> of <typeparamref name="TEntity" />
    /// </summary>
    public IEnumerable<TEntity> FindAll(PaginationRequest page, bool track = false)
    {
        return BuildQuery<object>(page, track: track).ToList();
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="PageSize" /> of <typeparamref name="TEntity" />
    ///     that matches the provided <paramref name="predicate" />.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicate,
        bool track = false
    )
    {
        return BuildQuery<object>(page, predicate, track: track).ToList();
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="PageSize" /> of <typeparamref name="TEntity" />
    ///     ordered by <typeparamref name="TOrderKey" />.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false
    )
    {
        return BuildQuery(page, orderKey: orderKey, sort: sort, track: track).ToList();
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="PageSize" /> of <typeparamref name="TEntity" />
    ///     that matches the provided <paramref name="predicate" /> and ordered by <typeparamref name="TOrderKey" />
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false
    )
    {
        return BuildQuery(page, predicate, orderKey, sort, track).ToList();
    }


    /// <summary>
    ///     Retrives all records held in <typeparamref name="TEntity" /> entity's table.
    /// </summary>
    public async Task<IEnumerable<TEntity>> FindAllAsync(bool track = false,
        CancellationToken cancellationToken = default)
    {
        return track switch
        {
            true => await _context.Set<TEntity>().ToListAsync(cancellationToken),
            false => await _context.Set<TEntity>().AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken)
        };
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="PageSize" /> of <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync(PaginationRequest page,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync<object>(page, track: track, cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="PageSize" /> of <typeparamref name="TEntity" /> fullfilling
    ///     the given <paramref name="predicate" /> condition.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicate,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync<object>(
                page,
                predicate,
                track: track,
                cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="page.PageSize" /> of <typeparamref name="TEntity" />
    ///     ordered by <typeparamref name="TOrderKey" />.
    /// </summary>
    /// <param name="page"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync(
                page,
                orderKey: orderKey,
                sort: sort,
                track: track,
                cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);
    }


    /// <summary>
    ///     Returns a given set of at most <paramref name="PageSize" /> of <typeparamref name="TEntity" />
    ///     that matches the provided <paramref name="predicate" /> and ordered by <typeparamref name="TOrderKey" />
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync(
                page,
                predicate,
                orderKey,
                sort,
                track,
                cancellationToken))
            .ToListAsync(cancellationToken);
    }

    #region Query builder methods

    /// <summary>
    ///     Helper function used to build a query for <typeparamref name="TEntity" /> entity.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="predicate"></param>
    /// <param name="orderKey"></param>
    /// <param name="sort"></param>
    /// <param name="track"></param>
    /// <returns></returns>
    private IQueryable<TEntity> BuildQuery<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, TOrderKey>>? orderKey = null,
        SortDirection sort = SortDirection.Ascending,
        bool track = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (predicate != null) query = query.Where(predicate);

        if (orderKey != null)
            query = sort == SortDirection.Ascending ? query.OrderBy(orderKey) : query.OrderByDescending(orderKey);

        query = query.Skip(page.Skip).Take(page.Offset);

        if (!track) query = query.AsNoTrackingWithIdentityResolution();

        return query;
    }

    private async Task<IQueryable<TEntity>> BuildQueryAsync<TOrderKey>(
        PaginationRequest page,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, TOrderKey>>? orderKey = null,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (predicate != null) query = query.Where(predicate);

        if (orderKey != null)
            query = sort == SortDirection.Ascending ? query.OrderBy(orderKey) : query.OrderByDescending(orderKey);

        query = query.Skip(page.Skip).Take(page.Offset);

        if (!track) query = query.AsNoTrackingWithIdentityResolution();

        return query;
    }

    #endregion
}