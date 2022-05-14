namespace Cache.NET {

	public class CacheEntry {
		public string Key;
		public object Value;
		public DateTimeOffset ExpiresAt;

		public CacheEntry(string key, object value, DateTimeOffset expiresAt) {
			Key = key;
			Value = value;
			ExpiresAt = expiresAt;
		}
	}

}
