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
        public virtual string getPrimaryKey()
        {
            return "id";            
        }

        public virtual string getTableName()
        {
            return "";
        }

        virtual public async Task<List<T>> getAllLoaded<T>(WrathIncarnateContext context, int pageNumber = 1, int pageSize = 100) {
            return await getAll<T>(context, pageNumber, pageSize);
        }

        virtual public async Task<List<T>> getAll<T>(WrathIncarnateContext context, int pageNumber = 1, int pageSize = 100) {
            if (pageSize > 1000) {
                pageSize = 1000;
            }

            var tableName  = getTableName();
            var offset     = pageNumber == 1 ? 1: pageSize * (pageNumber - 1);
            var primaryKey = getPrimaryKey();

            var query = $"{SEARCH} {tableName} e ORDER BY e.{primaryKey} DESC LIMIT {pageSize} OFFSET {offset}";
            var results = await context.connection.QueryAsync<T>(query);

            return results.ToList();
        }

        virtual public async Task<T> getById<T>(WrathIncarnateContext context, long id) {
            var tableName  = getTableName();
            var primaryKey = getPrimaryKey();

            var query = $"{SEARCH} {tableName} e WHERE e.{primaryKey} = @id ORDER BY e.{primaryKey} DESC LIMIT 1";

            return await context.connection.QueryFirstOrDefaultAsync<T>(query, new { id = id });
        }
    }
}
