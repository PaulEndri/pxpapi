using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PixelPubApi.MySQL;

namespace PixelPubApi.Interfaces {
    public interface IModel {
        string getPrimaryKey();
        string getTableName();
        string getInsertQuery();

        string getPutQuery(long id);
        Task<T> create<T>(T record);
        Task<T> replace<T>(long id, T record);
        Task<List<T>> getAll<T>(int pageNumber = 1, int pageSize = 100);
        Task<List<T>> getAllLoaded<T>(int pageNumber = 1, int pageSize = 100);
        Task<T> getById<T>(long id);
    }
}
