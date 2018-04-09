#region

using System;
using System.Linq;
using System.Threading.Tasks;
using PixelPubApi.Interfaces;
using PixelPubApi.MySQL;
using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#endregion

namespace PixelPubApi.Repository.Api
{
    public class ApiRepository<TEntity> : IRepository<TEntity> where TEntity : class, IModel
    {
        protected readonly WrathIncarnateContext _context;
        public ApiRepository(WrathIncarnateContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> GetAll(DateTime? dateCreatedBefore, DateTime? dateCreatedAfter, int pageNumber = 1, int pageSize = 100)
        {
            if(pageSize > 1000) {
                pageSize = 1000;
            }

            var model     = typeof(TEntity);
            var tableName = _context.Model.FindEntityType(model).SqlServer().TableName;
            var offset    = pageSize * (pageNumber - 1);
            var entity    = (TEntity)typeof(TEntity).GetConstructors()[0].Invoke(null);
            var dateQuery = entity.getDateFilterSubQuery(dateCreatedBefore, dateCreatedAfter);

            var query = $"SELECT * FROM {tableName} e";

            if(!string.IsNullOrWhiteSpace(dateQuery)) {
                query += " WHERE " + dateQuery;
            }

            query += " ORDER BY e.id DESC LIMIT {0} OFFSET {1}";
            offset = offset == 0 ? 1 : offset;

            return _context
                .Set<TEntity>()
                .FromSql(query, pageSize, offset)
                .AsNoTracking();
        }

        public async Task<TEntity> GetById(int id)
        {
            await Task.Delay(1);

            return _context.Set<TEntity>().Find((long)id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        
        public async Task Update(int id, TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }
        
        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
