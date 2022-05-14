namespace Cache.NET {

	public interface ICache {
		void Add<T>(string key, T value, TimeSpan duration);
		void Set<T>(string key, T value, TimeSpan duration);
		void Remove(string key);
		CacheEntry Get<T>(string key);
	}

}
