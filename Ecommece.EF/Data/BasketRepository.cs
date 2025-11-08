using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommece.EF.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var data = await _cache.GetStringAsync(basketId);
            return string.IsNullOrEmpty(data) ? null : JsonSerializer.Deserialize<CustomerBasket>(data, _jsonOptions);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var serialized = JsonSerializer.Serialize(basket);
            // set expiry if you want (e.g., 7 days)
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            };
            await _cache.SetStringAsync(basket.Id, serialized, options);
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            await _cache.RemoveAsync(basketId);
            return true;
        }
    }
}
