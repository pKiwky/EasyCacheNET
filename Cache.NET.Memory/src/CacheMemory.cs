using System.Collections.Concurrent;
namespace Cache.NET.Memory {

	public class CacheMemory : ICacheMemory {
		private readonly CacheMemoryOptions _options;
		private DateTimeOffset _lastTimeScan;
		private ConcurrentDictionary<string, CacheEntry> _cache;

		public CacheMemory(CacheMemoryOptions option) {
			_cache = new();
			_options = option;
			_lastTimeScan = DateTimeOffset.UtcNow;
		}

		public void Add<T>(string key, T value, TimeSpan duration) {
			AddInternal(key, value, duration, false);

#if DEBUG
			Console.WriteLine("[Cache {0}] Add Key: {1}, Duration: {2}s", DateTimeOffset.UtcNow.UtcDateTime, key, duration.TotalSeconds);
#endif
		}

		public void Set<T>(string key, T value, TimeSpan duration) {
			throw new NotImplementedException();
		}

		public CacheEntry Get<T>(string key) {
			throw new NotImplementedException();
		}

		public void Remove(string key) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Cache new entry.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="duration"></param>
		/// <param name="replace"></param>
		/// <returns></returns>
		private bool AddInternal<T>(string key, T value, TimeSpan duration, bool replace = false) {
			var cacheEntry = new CacheEntry() {
				Key = key,
				Value = value,
				ExpiresAt = Utility.IncrementDate(duration)
			};

			// The cache entry date is in past.
			if (cacheEntry.ExpiresAt < DateTime.UtcNow) {
				return false;
			}

			if (replace == true) {
				_cache.AddOrUpdate(key, cacheEntry, (entry, oldEntry) => cacheEntry);
			} else {
				if (_cache.TryAdd(key, cacheEntry) == false) {
					return false;
				}
			}

			ScanCacheKeys();
			return true;
		}

		/// <summary>
		/// Scan entire cache and remove expired entries.
		/// </summary>
		private void ScanCacheKeys() {
			var currentTime = DateTimeOffset.UtcNow;
			long lastScanDiff = (long)(currentTime - _lastTimeScan).TotalSeconds;

			if (lastScanDiff > _options.EntryScanInterval) {
				_lastTimeScan = currentTime;

				Task.Factory.StartNew(() => {
					var expiredCache = _cache.Values.Where(x => x.ExpiresAt < currentTime);

					foreach (var entry in expiredCache) {
						Remove(entry.Key);
					}
				});
			}
		}


	}

}
