using EfLight.Utils;

using System.Linq.Expressions;

namespace EfLight.Abstractions;
internal interface ICrudRepository<TEntity, TKey> where TEntity : class
{
    #region Save fns
    int SaveChanges();

    Task<int> SaveChangesAsync();
    #endregion


    #region Create fns
    public int Add(TEntity entity);

    public Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);


    public int AddMany(IEnumerable<TEntity> entities);


    //public Task<int> AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    #endregion


    #region Read fns
    public Optional<TEntity> FindById(TKey id);

    public Task<Optional<TEntity>> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);

    public Optional<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicateFn);

    public Task<Optional<TEntity>> FindWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);

    public long Count();

    public Task<long> CountAsync(CancellationToken cancellationToken = default);

    public long CountWhere(Expression<Func<TEntity, bool>> predicateFn);

    public Task<long> CountWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);

    public bool ExistsWhere(Expression<Func<TEntity, bool>> predicateFn);
    #endregion


    #region Delete fns
    public int DeleteById(TKey id);

    public Task<int> DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

    public int DeleteWhere(Expression<Func<TEntity, bool>> predicateFn);

    public Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicateFn, CancellationToken cancellationToken = default);
    #endregion


    #region Update fns
    public int Update(TEntity entity);

    public int UpdateMany(IEnumerable<TEntity> entities);
    #endregion
}
