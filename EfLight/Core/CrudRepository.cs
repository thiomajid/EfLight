using EfLight.Abstractions;
using EfLight.Extensions;
using EfLight.Utils;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace EfLight.Core;

public abstract class CrudRepository<TEntity, TKey> : LightRepository, ICrudRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// The <see cref="DbSet{TEntity}"/> related to this repository.
    /// </summary>
    protected DbSet<TEntity> _set { get => _context.Set<TEntity>(); }

    protected CrudRepository(DbContext context) : base(context)
    {
    }


    #region Save fns
    /// <summary>
    /// Commits changes to the database.
    /// </summary>
    /// <returns>The number of modified entries.</returns>
    public int SaveChanges() => _context.SaveChanges();


    /// <summary>
    /// Commits changes to the database.
    /// </summary>
    /// <returns>The number of modified entries.</returns>
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    #endregion


    #region Count fns
    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public long Count() => _context.Set<TEntity>().LongCount();


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public async Task<long> CountAsync(CancellationToken cancellationToken = default) =>
        await _context.Set<TEntity>().LongCountAsync(cancellationToken);


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table
    /// fullfilling the given <paramref name="predicateFn"/>'s condition.
    /// </summary>
    /// <param name="predicateFn">A condition that every <typeparamref name="TEntity"/> can fullfill</param>
    public long CountWhere(Expression<Func<TEntity, bool>> predicateFn) =>
        _context.Set<TEntity>().LongCount(predicateFn);


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table
    /// fullfilling the given <paramref name="predicateFn"/>'s condition.
    /// </summary>
    /// <param name="predicateFn"></param>
    public async Task<long> CountWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default) =>
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
    public int DeleteById(TKey id)
    {
        TEntity? entity = _context.Set<TEntity>().Find(id);

        if (entity is not null)
        {
            _context.Remove(entity);
            return 1;
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
    public async Task<int> DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _context.Set<TEntity>().FindAsync(id, cancellationToken);

        if (entity is not null)
        {
            _context.Remove(entity);
            return 1;
        }
        throw new NullReferenceException(message: $"No entity has the key {id} in the table of {nameof(TEntity)}");
    }


    /// <summary>
    /// Deletes one or more <typeparamref name="TEntity"/> entities based on the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    /// <returns>The number of removed rows.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public int DeleteWhere(Expression<Func<TEntity, bool>> predicateFn)
    {
        IEnumerable<TEntity> entities = _context.Set<TEntity>().Where(predicateFn);

        if (entities.Any())
        {
            _context.RemoveRange(entities);
            return entities.Count();
        }
        throw new NullReferenceException($"One of the provided ID is invalid.");
    }


    /// <summary>
    /// Deletes one or more <typeparamref name="TEntity"/> entities based on the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    /// <returns>The number of removed rows.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default)
    {
        IEnumerable<TEntity> entities = _context.Set<TEntity>().Where(predicateFn);

        if (entities.Any())
        {
            _context.RemoveRange(entities);
            return Task.FromResult(entities.Count());
        }
        throw new NullReferenceException($"One of the provided ID is invalid.");
    }
    #endregion


    #region Existence fns
    /// <summary>
    /// Checks if at least one <typeparamref name="TEntity"/> fullfills the condition of
    /// the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public bool ExistsWhere(Expression<Func<TEntity, bool>> predicateFn) => _context.Set<TEntity>().Any(predicateFn);


    /// <summary>
    /// Checks if at least one <typeparamref name="TEntity"/> fullfills the condition of
    /// the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public async Task<bool> ExistsWhereAsync(Expression<Func<TEntity, bool>> predicateFn) => await _context.Set<TEntity>().AnyAsync(predicateFn);


    /// <summary>
    /// Checks if all <see cref="TEntity"/> fullfills the given <paramref name="predicateFn"/>
    /// </summary>
    /// <param name="predicateFn"></param>
    public bool AllAre(Expression<Func<TEntity, bool>> predicateFn) => _context.Set<TEntity>().All(predicateFn);


    /// <summary>
    /// Checks if all <see cref="TEntity"/> fullfills the given <paramref name="predicateFn"/>
    /// </summary>
    /// <param name="predicateFn"></param>
    public async Task<bool> AllAreAsync(Expression<Func<TEntity, bool>> predicateFn) => await _context.Set<TEntity>().AllAsync(predicateFn);
    #endregion


    #region Select fns
    /// <summary>
    /// Finds the <typeparamref name="TEntity"/>'s having the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    public Optional<TEntity> FindById(TKey id) =>
        Optional<TEntity>.OfNullable(_context.Set<TEntity>().Find(id));


    /// <summary>
    /// Finds the <typeparamref name="TEntity"/>'s having the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    public async Task<Optional<TEntity>> FindByIdAsync(TKey id, CancellationToken cancellationToken = default) =>
        Optional<TEntity>.OfNullable(await _context.Set<TEntity>().FindAsync(id, cancellationToken));


    /// <summary>
    /// Finds the <strong>first</strong> <typeparamref name="TEntity"/> entity that matches
    /// the given <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public Optional<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicateFn)
    {
        TEntity? result = _context.Set<TEntity>().Where(predicateFn)
            .FirstOrDefault();

        return Optional<TEntity>.OfNullable(result);
    }


    /// <summary>
    /// Finds the <strong>first</strong> <typeparamref name="TEntity"/> entity that matches
    /// the given <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public async Task<Optional<TEntity>> FindWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default)
    {
        TEntity? result = await _context.Set<TEntity>().Where(predicateFn)
            .FirstOrDefaultAsync(cancellationToken);

        return Optional<TEntity>.OfNullable(result);
    }
    #endregion


    #region Create fns
    /// <summary>
    /// Persists the given <typeparamref name="TEntity"/> entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>
    /// <strong>1</strong> if the entity has been saved. Otherwise,
    /// it will be <strong>0</strong>.
    /// </returns>
    public int Add(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            return 1;
        }
        catch (Exception ex)
        {
            ex.StackTrace?.ToConsole();
            return 0;
        }
    }


    /// <summary>
    /// Persists the given <typeparamref name="TEntity"/> entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>
    /// <strong>1</strong> if the entity has been saved. Otherwise,
    /// it will be <strong>0</strong>.
    /// </returns>
    public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            return 1;
        }
        catch (Exception ex)
        {
            ex.StackTrace?.ToConsole();
            return 0;
        }
    }


    /// <summary>
    /// Persists many <typeparamref name="TEntity"/> entities to the database.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>
    /// The number of added records. If an error occurs, then the function will return 0.
    /// </returns>
    public int AddMany(IEnumerable<TEntity> entities)
    {
        try
        {
            _context.Set<TEntity>().AddRange(entities);
            return entities.Count();
        }
        catch (Exception ex)
        {
            ex.StackTrace?.ToConsole();
            return 0;
        }
    }


    /// <summary>
    /// Persists many <typeparamref name="TEntity"/> entities to the database.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>
    /// The number of added records. If an error occurs, then the function will return 0.
    /// </returns>
    //public Task<int> AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}
    #endregion


    #region Update fns
    /// <summary>
    /// Updates data related to <typeparamref name="TEntity"/>'s entity.
    /// </summary>
    /// <param name="entity"></param>
    public int Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return 1;
    }


    /// <summary>
    /// Updates data related to a set of <typeparamref name="TEntity"/> entities.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public int UpdateMany(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        return entities.Count();
    }
    #endregion
}
