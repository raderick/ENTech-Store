﻿using System;
using ENTech.Store.Infrastructure.Cache;

namespace ENTech.Store.Services.SharedModule.ModuleLogic
{
	public class TokenUtilService
	{
		private readonly IDistributedCache _cache;

		public TokenUtilService(IDistributedCache cache)
		{
			_cache = cache;
		}

		public TokenData Create(object associatedObject)
		{
			var token = Guid.NewGuid().ToString();
			var expireAt = DateTime.UtcNow;
			expireAt = expireAt.AddDays(2);

			var opts = new CacheOpts() {ExpireAt = expireAt, IsSlidingExpireIn = false};

			_cache.Set(GetCacheKey(token), associatedObject, opts);

			var data = new TokenData();

			data.Token = token;
			data.ExpiresAt = expireAt;
			return data;
		}

		public T GetByToken<T>(string token)
		{
			T cacheObject = default(T);
			if (_cache.TryGet(GetCacheKey(token), ref cacheObject))
			{
				return cacheObject;
			}
			return default(T);
		}

		private string GetCacheKey(string token)
		{
			return String.Format("EventGrid_API2_Token_{0}", token);
		}
	}
}