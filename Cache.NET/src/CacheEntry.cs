namespace Cache.NET {

	public class CacheEntry {
		public string Key;
		public object Value;
		public DateTimeOffset ExpiresAt;

		public CacheEntry() { }

		public CacheEntry(string key, object value, DateTimeOffset expiresAt) {
			Key = Key;
			Value = value;
			ExpiresAt = expiresAt;
		}
	}

}
