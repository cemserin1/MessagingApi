using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApi.Service.Caching
{
    public interface ICacheService
    {
        Task<TData> GetDataFromCache<TData>(string key);
        Task AddDataToCache<TData>(string key, TData data, TimeSpan? timeSpan = null);
    }
}
