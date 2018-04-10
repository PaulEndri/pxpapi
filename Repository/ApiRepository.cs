#region

using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PixelPubApi.Interfaces;
using PixelPubApi.MySQL;
using MySql.Data.MySqlClient;

#endregion

namespace PixelPubApi.Repository.Api
{
    public class ApiRepository<TEntity> : IRepository<TEntity> where TEntity : class, IModel
    {
        protected readonly WrathIncarnateContext _context;
        public ApiRepository(WrathIncarnateContext context)
        {
            context.connection.Open();
            _context = context;
        }

        public async Task<List<TEntity>> GetAllLoaded(int pageNumber = 1, int pageSize = 100) {
            if (pageSize > 1000) {
                pageSize = 1000;
            }

            var entity = (TEntity)typeof(TEntity).GetConstructors()[0].Invoke(new []{_context});

            return await entity.getAllLoaded<TEntity>(pageNumber, pageSize);
        }

        public async Task<List<TEntity>> GetAll(int pageNumber = 1, int pageSize = 100)
        {
            if(pageSize > 1000) {
                pageSize = 1000;
            }

            var entity     = (TEntity)typeof(TEntity).GetConstructors()[0].Invoke(new[] { _context });

            return await entity.getAll<TEntity>(pageNumber, pageSize);
        }

        public async Task<TEntity> GetById(int id)
        {
            var entity = (TEntity)typeof(TEntity).GetConstructors()[0].Invoke(new []{_context});

            return await entity.getById<TEntity>(id);
        }

        public async Task<TEntity> Create(TEntity record)
        {
            var entity = (TEntity)typeof(TEntity).GetConstructors()[0].Invoke(new []{_context});

            return await entity.create(record);;
        }

        public async Task<TEntity> Replace(long id, TEntity record)
        {
            var entity = (TEntity)typeof(TEntity).GetConstructors()[0].Invoke(new[] { _context });

            return await entity.replace(id, record);
        }
    }
}
