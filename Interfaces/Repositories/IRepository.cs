#region

using System.Linq;
using System.Threading.Tasks;
using System;

#endregion

namespace PixelPubApi.Interfaces {
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(DateTime? dateCreatedBefore, DateTime? dateCreatedAfter, int pageSize, int pageNumber);

        Task<TEntity> GetById(int id);

        Task<TEntity> Create(TEntity entity);

        Task Update(int id, TEntity entity);

        Task Delete(int id);
    }
}
