#region

using Dapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PixelPubApi.Models.Entities;
using PixelPubApi.Interfaces;
using PixelPubApi.MySQL;

#endregion

namespace PixelPubApi.Providers
{
    public class ApiAccessProvider : IApiAccessProvider
    {
        private WrathIncarnateContext _context;
        public const string searchKey = "SELECT * FROM api_key a WHERE a.key = @key";
        public ApiAccessProvider(WrathIncarnateContext context)
        {
            _context = context;
        }

        public async Task<ApiAccess> getByKey(string key)
        {
            _context.connection.Open();

            return await _context.connection.QueryFirstOrDefaultAsync<ApiAccess>(searchKey, new {key = key});
        }
    }
}