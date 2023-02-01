using System.Linq.Expressions;
using EfLight.Abstractions;
using EfLight.Utils;

using Microsoft.EntityFrameworkCore;

namespace EfLight.Core;
public abstract class PagingAndSortingRepository<TEntity, TKey> : CrudRepository<TEntity,TKey>, IPagingAndSortingRepository<TEntity, TKey>
    where TEntity : class
{
    protected PagingAndSortingRepository(DbContext context) : base(context)
    {

    }

    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public IEnumerable<TEntity> FindAll() => _context.Set<TEntity>().AsNoTrackingWithIdentityResolution().ToList();


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// </summary>
    public IEnumerable<TEntity> FindAll(PageRequest page) => _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .Skip(page.PageNumber * page.PageSize)
        .Take(page.PageSize)
        .ToList();


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll(PageRequest page, Expression<Func<TEntity, bool>> predicateFn) =>
        _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .Where(predicateFn)
        .Skip(page.PageNumber * page.PageSize)
        .Take(page.PageSize)
        .ToList();


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll<TOrderKey>(PageRequest page, Expression<Func<TEntity, TOrderKey>> orderKey) =>
        _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .Skip(page.PageNumber * page.PageSize)
        .Take(page.PageSize)
        .OrderBy(orderKey)
        .ToList();


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public IEnumerable<TEntity> FindAll<TOrderKey>(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, Expression<Func<TEntity, TOrderKey>> orderKey) =>
        _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .Where(predicateFn)
        .Skip(page.PageNumber * page.PageSize)
        .Take(page.PageSize)
        .OrderBy(orderKey)
        .ToList();


    /// <summary>
    /// Retrives all records held in <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public async Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default) =>
       await _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .ToListAsync(cancellationToken);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync(PageRequest page, CancellationToken cancellationToken = default) =>
        await _context.Set<TEntity>()
            .AsNoTrackingWithIdentityResolution()
            .Skip(page.PageNumber * page.PageSize)
            .Take(page.PageSize)
            .ToListAsync();


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> fullfilling
    /// the given <paramref name="predicateFn"/> condition.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default) =>
        await _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .Skip(page.PageNumber * page.PageSize)
        .Take(page.PageSize)
        .ToListAsync(cancellationToken);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// ordered by <typeparamref name="TOrderKey"/>.
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(PageRequest page, Expression<Func<TEntity, TOrderKey>> orderKey, CancellationToken cancellationToken = default) =>
       await _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .Skip(page.PageNumber * page.PageSize)
        .Take(page.PageSize)
        .OrderBy(orderKey)
        .ToListAsync(cancellationToken);


    /// <summary>
    /// Returns a given set of at most <paramref name="PageSize"/> of <typeparamref name="TEntity"/> 
    /// that matches the provided <paramref name="predicateFn"/> and ordered by <typeparamref name="TOrderKey"/>
    /// </summary>
    /// <param name="PageNumber"></param>
    /// <param name="PageSize"></param>
    public async Task<IEnumerable<TEntity>> FindAllAsync<TOrderKey>(PageRequest page, Expression<Func<TEntity, bool>> predicateFn, Expression<Func<TEntity, TOrderKey>> orderKey, CancellationToken cancellationToken = default) =>
       await _context.Set<TEntity>()
        .AsNoTrackingWithIdentityResolution()
        .Where(predicateFn)
        .Skip(page.PageNumber * page.PageSize)
        .Take(page.PageSize)
        .OrderBy(orderKey)
        .ToListAsync(cancellationToken);
}
