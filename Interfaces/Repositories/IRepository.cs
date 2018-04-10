#region

using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

#endregion

namespace PixelPubApi.Interfaces {
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllLoaded(int pageNumber = 1, int pageSize = 100);
        Task<List<TEntity>> GetAll(int pageSize, int pageNumber);

        Task<TEntity> GetById(int id);
    }
}
