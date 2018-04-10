using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PixelPubApi.MySQL;

namespace PixelPubApi.Interfaces {
    public interface IModel {
        string getPrimaryKey();
        string getTableName();
        Task<List<T>> getAll<T>(WrathIncarnateContext context, int pageNumber = 1, int pageSize = 100);
        Task<List<T>> getAllLoaded<T>(WrathIncarnateContext context, int pageNumber = 1, int pageSize = 100);
        Task<T> getById<T>(WrathIncarnateContext context, long id);
    }
}
