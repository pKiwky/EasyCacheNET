namespace Cache.NET {
	/// <summary>
	/// Event fired when new entry are added in cache.
	/// </summary>
	/// <param name="entry">Cache entry</param>
	/// <param name="replace">If key was replace then true, else false</param>
	public delegate void OnCacheEntryAddDelegate(CacheEntry entry, bool replace);

	/// <summary>
	/// Event fired when cache entry are removed from cache.
	/// </summary>
	/// <param name="entry">Cache entry</param>
	/// <param name="expired">If entry expired then true, else false (when removed manually)</param>
	public delegate void OnCacheEntryRemoveDelegate(CacheEntry entry, bool expired);

	public interface ICache {
		public event OnCacheEntryAddDelegate OnCacheEntryAdd;
		public event OnCacheEntryRemoveDelegate OnCacheEntryRemove;

		void Add<T>(string key, T value, TimeSpan duration);
		void Set<T>(string key, T value, TimeSpan duration);
		void Remove(string key);
		CacheEntry Get<T>(string key);
	}

}
