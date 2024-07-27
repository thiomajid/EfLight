using System.Linq.Expressions;
using EfLight.Abstractions;
using EfLight.Extensions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace EfLight.Core;

public abstract class CrudRepository<TEntity, TKey>(DbContext context)
    : LightRepository(context), ICrudRepository<TEntity, TKey>
    where TEntity : class
{
    #region Save fns

    /// <summary>
    /// Commits changes to the database.
    /// </summary>
    /// <returns>The number of modified entries.</returns>
    public virtual int SaveChanges() => _context.SaveChanges();


    /// <summary>
    /// Commits changes to the database.
    /// </summary>
    /// <returns>The number of modified entries.</returns>
    public virtual async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    #endregion


    #region Count fns

    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public virtual long Count() => _context.Set<TEntity>().LongCount();


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public virtual async Task<long> CountAsync(CancellationToken cancellationToken = default) =>
        await _context.Set<TEntity>().LongCountAsync(cancellationToken);


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table
    /// fullfilling the given <paramref name="predicateFn"/>'s condition.
    /// </summary>
    /// <param name="predicateFn">A condition that every <typeparamref name="TEntity"/> can fullfill</param>
    public virtual long CountWhere(Expression<Func<TEntity, bool>> predicateFn) =>
        _context.Set<TEntity>().LongCount(predicateFn);


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table
    /// fullfilling the given <paramref name="predicateFn"/>'s condition.
    /// </summary>
    /// <param name="predicateFn"></param>
    public virtual async Task<long> CountWhereAsync(Expression<Func<TEntity, bool>> predicateFn,
        CancellationToken cancellationToken = default) =>
        await _context.Set<TEntity>().LongCountAsync(predicateFn, cancellationToken);

    #endregion


    #region Delete fns

    /// <summary>
    /// Deletes a given <typeparamref name="TEntity"/> entity based on its <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// <strong>1</strong> if the <typeparamref name="TEntity"/> entity has been removed.
    /// Otherwise, it will throw a <see cref="NullReferenceException"/>
    /// </returns>
    /// <exception cref="NullReferenceException">
    /// Thrown if no entry matches the given <paramref name="id"/>
    /// </exception>
    public virtual EntityEntry<TEntity> DeleteById(TKey id)
    {
        var entity = _context.Set<TEntity>().Find(id);
        if (entity is not null)
        {
            return _context.Remove(entity);
        }

        throw new NullReferenceException(message: $"No entity has the key {id} in the table of {nameof(TEntity)}");
    }


    /// <summary>
    /// Deletes a given <typeparamref name="TEntity"/> entity based on its <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// <strong>1</strong> if the <typeparamref name="TEntity"/> entity has been removed.
    /// Otherwise, it will throw a <see cref="NullReferenceException"/>
    /// </returns>
    /// <exception cref="NullReferenceException">
    /// Thrown if no entry matches the given <paramref name="id"/>
    /// </exception>
    public virtual async Task<EntityEntry<TEntity>> DeleteByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _context.Set<TEntity>().FindAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new NullReferenceException(message: $"No entity has the key {id} in the table of {nameof(TEntity)}");
        }

        return _context.Remove(entity);
    }


    /// <summary>
    /// Deletes one or more <typeparamref name="TEntity"/> entities based on the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    /// <returns>The number of removed rows.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public virtual int DeleteWhere(Expression<Func<TEntity, bool>> predicateFn)
    {
        return _context.Set<TEntity>().Where(predicateFn).ExecuteDelete();
    }


    /// <summary>
    /// Deletes one or more <typeparamref name="TEntity"/> entities based on the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    /// <returns>The number of removed rows.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public virtual Task<int> DeleteWhereAsync(
        Expression<Func<TEntity, bool>> predicateFn,
        CancellationToken cancellationToken = default
    )
    {
        return _context.Set<TEntity>().Where(predicateFn).ExecuteDeleteAsync(cancellationToken);
    }

    #endregion


    #region Existence fns

    /// <summary>
    /// Checks if at least one <typeparamref name="TEntity"/> fullfills the condition of
    /// the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public virtual bool ExistsWhere(Expression<Func<TEntity, bool>> predicateFn) =>
        _context.Set<TEntity>().Any(predicateFn);


    /// <summary>
    /// Checks if at least one <typeparamref name="TEntity"/> fullfills the condition of
    /// the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public virtual async Task<bool> ExistsWhereAsync(Expression<Func<TEntity, bool>> predicateFn) =>
        await _context.Set<TEntity>().AnyAsync(predicateFn);


    /// <summary>
    /// Checks if all <see cref="TEntity"/> fullfills the given <paramref name="predicateFn"/>
    /// </summary>
    /// <param name="predicateFn"></param>
    public virtual bool AllAre(Expression<Func<TEntity, bool>> predicateFn) => _context.Set<TEntity>().All(predicateFn);


    /// <summary>
    /// Checks if all <see cref="TEntity"/> fullfills the given <paramref name="predicateFn"/>
    /// </summary>
    /// <param name="predicateFn"></param>
    public virtual async Task<bool> AllAreAsync(Expression<Func<TEntity, bool>> predicateFn) =>
        await _context.Set<TEntity>().AllAsync(predicateFn);

    #endregion


    #region Select fns

    /// <summary>
    /// Finds the <typeparamref name="TEntity"/>'s having the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    public virtual Option<TEntity> FindById(TKey id)
    {
        var entity = _context.Set<TEntity>().Find(id);
        return entity.ToOption();
    }


    /// <summary>
    /// Finds the <typeparamref name="TEntity"/>'s having the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    public virtual async Task<Option<TEntity>> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<TEntity>()
            .FindAsync(id, cancellationToken);

        return entity.ToOption();
    }


    /// <summary>
    /// Finds the <strong>first</strong> <typeparamref name="TEntity"/> entity that matches
    /// the given <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public virtual Option<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicateFn)
    {
        var entity = _context.Set<TEntity>()
            .Where(predicateFn)
            .FirstOrDefault();

        return entity.ToOption();
    }


    /// <summary>
    /// Finds the <strong>first</strong> <typeparamref name="TEntity"/> entity that matches
    /// the given <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public virtual async Task<Option<TEntity>> FindWhereAsync(
        Expression<Func<TEntity, bool>> predicateFn,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _context.Set<TEntity>()
            .Where(predicateFn)
            .FirstOrDefaultAsync(cancellationToken);

        return entity.ToOption();
    }

    #endregion


    #region Create fns

    /// <summary>
    /// Persists the given <typeparamref name="TEntity"/> entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    public virtual EntityEntry<TEntity> Add(TEntity entity)
    {
        return _context.Set<TEntity>().Add(entity);
    }


    /// <summary>
    /// Persists the given <typeparamref name="TEntity"/> entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    public virtual async Task<EntityEntry<TEntity>> AddAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }


    /// <summary>
    /// Persists many <typeparamref name="TEntity"/> entities to the database.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>
    /// The number of added records. If an error occurs, then the function will return 0.
    /// </returns>
    public virtual int AddMany(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        return entities.Count();
    }


    /// <summary>
    /// Persists many <typeparamref name="TEntity"/> entities to the database.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>
    /// The number of added records. If an error occurs, then the function will return 0.
    /// </returns>
    public virtual async Task<int> AddManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        return entities.Count();
    }

    #endregion


    #region Update fns

    /// <summary>
    /// Updates data related to <typeparamref name="TEntity"/>'s entity.
    /// </summary>
    /// <param name="entity"></param>
    public virtual int Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return 1;
    }


    /// <summary>
    /// Updates data related to a set of <typeparamref name="TEntity"/> entities.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual int UpdateMany(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        return entities.Count();
    }


    public virtual int UpdateWhere(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
    )
    {
        return _context.Set<TEntity>()
            .Where(predicate)
            .ExecuteUpdate(setPropertyCalls);
    }

    public virtual async Task<int> UpdateWhereAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken token = default
    )
    {
        return await _context.Set<TEntity>()
            .Where(predicate)
            .ExecuteUpdateAsync(setPropertyCalls, token);
    }

    #endregion
}