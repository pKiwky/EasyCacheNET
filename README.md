# EasyCacheNET


```c#
using EasyCacheNET.Memory;

class Program {
	private static CacheMemory _cache = new CacheMemory(new CacheMemoryOptions());

	public static void Main(String[] args) {
		_cache.OnCacheEntryAdd += (entry, replace) => {
			Console.WriteLine("[Cache] Add Key {0}, Expires: {1}s", entry.Key, entry.ExpiresAt);
		};
		_cache.OnCacheEntryRemove += (entry, expired) => {
			Console.WriteLine("[Cache] Remove Key {0}, Expires: {1}s", entry.Key, entry.ExpiresAt);
		};

		Task.Factory.StartNew(async () => {
			int x = 0;

			while (true) {
				_cache.Add(x.ToString(), $"Value - {x++}", TimeSpan.FromSeconds(15));
				await Task.Delay(500);
			}
		});

		while (true) {
			string value = Console.ReadLine();
			Console.WriteLine("Cache value '{0}' = {1}", value, _cache.Get<string>(value) ?? "DELETED VALUE");
		}
	}
}
```
