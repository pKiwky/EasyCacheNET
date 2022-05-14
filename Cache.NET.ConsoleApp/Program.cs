using Cache.NET.Memory;

class Program {
	public static void Main(String[] args) {
		CacheMemory c = new CacheMemory(new CacheMemoryOptions() { EntryScanInterval = 60 });

		c.Add("SAL", "1", TimeSpan.FromSeconds(5));
	}
}
