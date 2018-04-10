#region

using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

#endregion

namespace PixelPubApi.Interfaces {
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAll(int pageSize, int pageNumber);

        Task<TEntity> GetById(int id);
    }
}
