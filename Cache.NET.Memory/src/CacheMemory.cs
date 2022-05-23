using System.Collections.Concurrent;

namespace EasyCacheNET.Memory {

	public class CacheMemory : ICacheMemory {
		public event OnCacheEntryAddDelegate OnCacheEntryAdd;
		public event OnCacheEntryRemoveDelegate OnCacheEntryRemove;

		private readonly CacheMemoryOptions _options;
		private DateTimeOffset _lastTimeScan;
		private ConcurrentDictionary<string, CacheEntry> _cache;

		public CacheMemory(CacheMemoryOptions option) {
			_cache = new();
			_options = option;
			_lastTimeScan = DateTimeOffset.UtcNow;
		}

		public void Add<T>(string key, T value, TimeSpan duration) {
			bool result = AddInternal(key, value, duration, false);
		}

		public void Set<T>(string key, T value, TimeSpan duration) {
			bool result = AddInternal(key, value, duration, true);
		}

		public T Get<T>(string key) {
			if (_cache.TryGetValue(key, out var cacheEntry) == false) {
				return default(T);
			}

			if (cacheEntry.ExpiresAt < DateTimeOffset.UtcNow) {
				RemoveInternal(key, true);
				return default(T);
			}

			return (T)Convert.ChangeType(cacheEntry.Value, typeof(T));
		}

		public void Remove(string key) {
			RemoveInternal(key, false);
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

			OnCacheEntryAdd?.Invoke(cacheEntry, replace);
			ScanCacheKeys();
			return true;
		}

		/// <summary>
		/// Remove cache entry from cache by key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="expired"></param>
		/// <returns></returns>
		private bool RemoveInternal(string key, bool expired) {
			bool result = _cache.Remove(key, out var cacheEntry);

			OnCacheEntryRemove?.Invoke(cacheEntry, expired);
			return result;
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
						RemoveInternal(entry.Key, true);
					}
				});
			}
		}

	}

}
