using Cache.NET.Memory;

class Program {
	public static void Main(String[] args) {
		CacheMemory c = new CacheMemory(new CacheMemoryOptions());

		c.OnCacheEntryAdd += (entry, replace) => {
			Console.WriteLine("[Cache] Add Key {0}, Expires: {1}s", entry.Key, entry.ExpiresAt);
		};
		c.OnCacheEntryRemove += (entry, expired) => {
			Console.WriteLine("[Cache] Remove Key {0}, Expires: {1}s", entry.Key, entry.ExpiresAt);
		};

		Task.Factory.StartNew(async () => {
			int x = 0;
			while (true) {
				x++;
				c.Add(x.ToString(), $"Value - {x}", TimeSpan.FromSeconds(15));

				await Task.Delay(500);
			}
		});

		while (true) {
			string value = Console.ReadLine();
			Console.WriteLine("Cache value '{0}' = {1}", value, c.Get<string>(value)?.Value);
		}
	}
}
