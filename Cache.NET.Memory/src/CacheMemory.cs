using System.Collections.Concurrent;
namespace Cache.NET.Memory {

	public class CacheMemory : ICacheMemory {
		private readonly CacheMemoryOptions _options;
		private ConcurrentDictionary<string, CacheEntry> _cache;

		public CacheMemory(CacheMemoryOptions option) {
			_options = option;
		}

		public void Set<T>(string key, T value, TimeSpan duration) {
			throw new NotImplementedException();
		}

		public void Add<T>(string key, T value, TimeSpan duration) {
			throw new NotImplementedException();
		}

		public CacheEntry Get<T>(string key) {
			throw new NotImplementedException();
		}

		private void AddInternal(CacheEntry entry, TimeSpan duration) {

		}
	}

}
