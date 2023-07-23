using System.Linq.Expressions;

using EfLight.Abstractions;
using EfLight.Utils;

using Microsoft.EntityFrameworkCore;

namespace EfLight.Core;
public abstract class PagingAndSortingRepository<TEntity, TKey> : CrudRepository<TEntity, TKey>, IPagingAndSortingRepository<TEntity, TKey>
    where TEntity : class
{


    protected PagingAndSortingRepository(DbContext context) : base(context)
    {

    }

    #region Query builder methods
    /// <summary>
    /// Helper function used to build a query for <typeparamref name="TEntity"/> entity.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="predicateFn"></param>
    /// <param name="orderKey"></param>
    /// <param name="sort"></param>
    /// <param name="track"></param>
    /// <returns></returns>
    private IQueryable<TEntity> BuildQuery<TOrderKey>(
    PageRequest page,
    Expression<Func<TEntity, bool>>? predicateFn = null,
    Expression<Func<TEntity, TOrderKey>>? orderKey = null,
    SortDirection sort = SortDirection.Ascending,
    bool track = false)
    {
        var query = _set.AsQueryable();

        if (predicateFn != null)
        {
            query = query.Where(predicateFn);
        }

        if (orderKey != null)
        {
            query = sort == SortDirection.Ascending ? query.OrderBy(orderKey) : query.OrderByDescending(orderKey);
        }

        query = query.Skip(page.PageNumber * page.PageSize).Take(page.PageSize);

        if (!track)
        {
            query = query.AsNoTrackingWithIdentityResolution();
        }

        return query;
    }

    private async Task<IQueryable<TEntity>> BuildQueryAsync<TOrderKey>(
      PageRequest page,
      Expression<Func<TEntity, bool>>? predicateFn = null,
      Expression<Func<TEntity, TOrderKey>>? orderKey = null,
      SortDirection sort = SortDirection.Ascending,
      bool track = false,
      CancellationToken cancellationToken = default)
    {
        var query = _set.AsQueryable();

        if (predicateFn != null)
        {
            query = query.Where(predicateFn);
        }

        if (orderKey != null)
        {
            query = sort == SortDirection.Ascending ? query.OrderBy(orderKey) : query.OrderByDescending(orderKey);
        }

        query = query.Skip(page.PageNumber * page.PageSize).Take(page.PageSize);

        if (!track)
        {
            query = query.AsNoTrackingWithIdentityResolution();
        }

        return query;
    }
    #endregion


    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public IEnumerable<TEntity> FindAll(bool track = false)
    {
        return track switch
        {
            true => _set.ToList(),
            false => _set.AsNoTrackingWithIdentityResolution().ToList()
        };
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// </summary>
    public IEnumerable<TEntity> FindAll(PageRequest page, bool track = false)
    {
        return BuildQuery<object>(page, track: track).ToList();
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, bool track = false)
    {
        return BuildQuery<object>(page, predicateFn: predicateFn, track: track).ToList();
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll<TOrderKey>(
        PageRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false)
    {
        return BuildQuery(page, orderKey: orderKey, sort: sort, track: track).ToList();
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll<TOrderKey>(
        PageRequest page,
        Expression<Func<TEntity, bool>> predicateFn,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false)
    {
        return BuildQuery(page, predicateFn: predicateFn, orderKey: orderKey, sort: sort, track: track).ToList();
    }


    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public async Task<IEnumerable<TEntity>> FindAllAsync(bool track = false, CancellationToken cancellationToken = default)
    {
        return track switch
        {
            true => await _set.ToListAsync(cancellationToken),
            false => await _set.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken)
        };
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync(PageRequest page,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync<object>(page: page, track: track, cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> fullfilling
    /// the given <paramref name="predicateFn"/> condition.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync(
        PageRequest page,
        Expression<Func<TEntity, bool>> predicateFn,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync<object>(
            page: page,
            predicateFn: predicateFn,
            track: track,
            cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="page.PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="page"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PageRequest page,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync(
            page: page,
            orderKey: orderKey,
            sort:sort,
            track: track,
            cancellationToken: cancellationToken))
           .ToListAsync(cancellationToken);
    }


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(
        PageRequest page,
        Expression<Func<TEntity, bool>> predicateFn,
        Expression<Func<TEntity, TOrderKey>> orderKey,
        SortDirection sort = SortDirection.Ascending,
        bool track = false,
        CancellationToken cancellationToken = default)
    {
        return await (await BuildQueryAsync(
            page: page,
            predicateFn:predicateFn,
            orderKey: orderKey,
            sort: sort,
            track: track,
            cancellationToken: cancellationToken))
           .ToListAsync(cancellationToken);
    }
}
