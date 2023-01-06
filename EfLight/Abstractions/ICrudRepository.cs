using EfLight.Utils;

using System.Linq.Expressions;

namespace EfLight.Abstractions;
public interface ICrudRepository<TEntity, TKey> where TEntity : class
{
    #region Save fns
    /// <summary>
    /// Commits changes to the database.
    /// </summary>
    /// <returns>The number of modified entries.</returns>
    int SaveChanges();


    /// <summary>
    /// Commits changes to the database.
    /// </summary>
    /// <returns>The number of modified entries.</returns>
    Task<int> SaveChangesAsync();
    #endregion


    #region Count fns
    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public long Count();


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table.
    /// </summary>
    public Task<long> CountAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table
    /// fullfilling the given <paramref name="predicateFn"/>'s condition.
    /// </summary>
    /// <param name="predicateFn">A condition that every <typeparamref name="TEntity"/> can fullfill</param>
    public long CountWhere(Expression<Func<TEntity, bool>> predicateFn);


    /// <summary>
    /// Counts the number of records in the <typeparamref name="TEntity"/> entity's table
    /// fullfilling the given <paramref name="predicateFn"/>'s condition.
    /// </summary>
    /// <param name="predicateFn"></param>
    public Task<long> CountWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);
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
    public int Add(TEntity entity);


    /// <summary>
    /// Persists the given <typeparamref name="TEntity"/> entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>
    /// <strong>1</strong> if the entity has been saved. Otherwise,
    /// it will be <strong>0</strong>.
    /// </returns>
    public Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);


    /// <summary>
    /// Persists many <typeparamref name="TEntity"/> entities to the database.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>
    /// The number of added records. If an error occurs, then the function will return 0.
    /// </returns>
    public int AddMany(IEnumerable<TEntity> entities);


    //public Task<int> AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    #endregion


    #region Select fns
    /// <summary>
    /// Finds the <typeparamref name="TEntity"/>'s having the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    public Optional<TEntity> FindById(TKey id);


    /// <summary>
    /// Finds the <typeparamref name="TEntity"/>'s having the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    public Task<Optional<TEntity>> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);


    /// <summary>
    /// Finds the <strong>first</strong> <typeparamref name="TEntity"/> entity that matches
    /// the given <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public Optional<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicateFn);


    /// <summary>
    /// Finds the <strong>first</strong> <typeparamref name="TEntity"/> entity that matches
    /// the given <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public Task<Optional<TEntity>> FindWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);
    #endregion


    #region Existence fns
    /// <summary>
    /// Checks if at least one <typeparamref name="TEntity"/> fullfills the condition of
    /// the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public bool ExistsWhere(Expression<Func<TEntity, bool>> predicateFn);


    /// <summary>
    /// Checks if at least one <typeparamref name="TEntity"/> fullfills the condition of
    /// the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    public Task<bool> ExistsWhereAsync(Expression<Func<TEntity, bool>> predicateFn);


    /// <summary>
    /// Checks if all <see cref="TEntity"/> fullfills the given <paramref name="predicateFn"/>
    /// </summary>
    /// <param name="predicateFn"></param>
    public bool AllAre(Expression<Func<TEntity, bool>> predicateFn);


    /// <summary>
    /// Checks if all <see cref="TEntity"/> fullfills the given <paramref name="predicateFn"/>
    /// </summary>
    /// <param name="predicateFn"></param>
    public Task<bool> AllAreAsync(Expression<Func<TEntity, bool>> predicateFn);
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
    public int DeleteById(TKey id);


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
    public Task<int> DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes one or more <typeparamref name="TEntity"/> entities based on the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    /// <returns>The number of removed rows.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public int DeleteWhere(Expression<Func<TEntity, bool>> predicateFn);


    /// <summary>
    /// Deletes one or more <typeparamref name="TEntity"/> entities based on the <paramref name="predicateFn"/>.
    /// </summary>
    /// <param name="predicateFn"></param>
    /// <returns>The number of removed rows.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);
    #endregion


    #region Update fns
    /// <summary>
    /// Updates data related to <typeparamref name="TEntity"/>'s entity.
    /// </summary>
    /// <param name="entity"></param>
    public int Update(TEntity entity);


    /// <summary>
    /// Updates data related to a set of <typeparamref name="TEntity"/> entities.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public int UpdateMany(IEnumerable<TEntity> entities);
    #endregion
}
