#region

using System;
using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PixelPubApi.Interfaces;
using PixelPubApi.MySQL;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

#endregion

namespace PixelPubApi.Models
{
    public abstract class Model : IModel
    {
        [JsonIgnore]
        public const string SEARCH = "SELECT * FROM ";
        [JsonIgnore]
        protected WrathIncarnateContext _context {get; set;}

        public Model(WrathIncarnateContext context)
        {
            _context = context;
        }

        public Model()
        {}
        
        public virtual string getInsertQuery() {
            return "";
        }

        public virtual string getPutQuery(long id) {
            return "";
        }

        public virtual string getPrimaryKey()
        {
            return "id";            
        }

        public virtual string getTableName()
        {
            return "";
        }

        public virtual async Task<List<T>> getAllLoaded<T>(int pageNumber = 1, int pageSize = 100)
        {
            return await getAll<T>(pageNumber, pageSize);
        }

        public virtual async Task<List<T>> getAll<T>(int pageNumber = 1, int pageSize = 100)
        {
            if (pageSize > 1000) {
                pageSize = 1000;
            }

            var tableName  = getTableName();
            var offset     = pageNumber == 1 ? 1: pageSize * (pageNumber - 1);
            var primaryKey = getPrimaryKey();

            var query = $"{SEARCH} {tableName} e ORDER BY e.{primaryKey} DESC LIMIT {pageSize} OFFSET {offset}";
            var results = await _context.connection.QueryAsync<T>(query);

            return results.ToList();
        }

        public virtual async Task<T> getById<T>(long id)
        {
            var tableName  = getTableName();
            var primaryKey = getPrimaryKey();

            var query = $"{SEARCH} {tableName} e WHERE e.{primaryKey} = @id ORDER BY e.{primaryKey} DESC LIMIT 1";

            return await _context.connection.QueryFirstOrDefaultAsync<T>(query, new { id = id });
        }

        public virtual async Task<T> create<T>(T record)
        {
            var query  = getInsertQuery() + "; SELECT LAST_INSERT_ID();";
            var result = await _context.connection.QueryMultipleAsync(query, record);
            var id  = result.Read<int>().First();

            return await getById<T>((long)id);
        }

        public virtual async Task<T> replace<T>(long id, T record)
        {
            await _context.connection.ExecuteAsync(getPutQuery(id), record);

            return await getById<T>(id);
        }
    }
}
