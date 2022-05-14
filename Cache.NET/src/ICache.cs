namespace Cache.NET {

	public interface ICache {
		void Set<T>(string key, T value, TimeSpan duration);
		void Add<T>(string key, T value, TimeSpan duration);
		CacheEntry Get<T>(string key);
	}

}
