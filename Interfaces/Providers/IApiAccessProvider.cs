using System;
using System.Threading.Tasks;
using PixelPubApi.Models.Entities;

namespace PixelPubApi.Interfaces {
    public interface IApiAccessProvider {
        Task<ApiAccess> getByKey(string key);
    }
}
