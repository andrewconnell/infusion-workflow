using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AnonReadSecureWriteWeb.Respositories {
  public interface IEntityRepository<T> : IDisposable {
    /// <summary>
    /// Get all items in the repository.
    /// </summary>
    /// <returns></returns>
    IQueryable<T> All { get; }
    IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

    /// <summary>
    /// Gets multiple items from the repository matching specific criteria.
    /// </summary>
    IQueryable<T> GetMany(Expression<Func<T, bool>> where);

    /// <summary>
    /// Gets a single item from the repository matching specific criteria.
    /// </summary>
    T GetOne(int id);
    T GetOne(Expression<Func<T, bool>> where);

    /// <summary>
    /// Adds or updates an entity.
    /// </summary>
    void InsertOrUpdate(T entity);

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    void Delete(int id);
  }
}