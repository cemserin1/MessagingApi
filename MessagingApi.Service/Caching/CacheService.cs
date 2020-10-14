using System;
using System.Threading.Tasks;
using MessagingApi.Service.Helpers;
using Microsoft.Extensions.Caching.Distributed;

namespace MessagingApi.Service.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ISerializerHelper _serializerHelper;
        public CacheService(IDistributedCache distributedCache, ISerializerHelper serializerHelper)
        {
            _distributedCache = distributedCache;
            _serializerHelper = serializerHelper;
        }

        public async Task<TData> GetDataFromCache<TData>(string key)
        {
            return await GetFromDistributedCache<TData>(key);
        }

        public async Task AddDataToCache<TData>(string key, TData data, TimeSpan? timeSpan = null)
        {
            await AddToDistributedCache(key, data, timeSpan);
        }


        private async Task<TData> GetFromDistributedCache<TData>(string key)
        {
            return _serializerHelper.Deserialize<TData>(await _distributedCache.GetAsync(key));
        }
        private async Task AddToDistributedCache<TData>(string key, TData data, TimeSpan? timeSpan = null)
        {
            await _distributedCache.SetAsync(key, _serializerHelper.Serialize(data));
        }


    }
}
