using Cache.NET.Memory;

class Program {
	public static void Main(String[] args) {
		CacheMemory c = new CacheMemory(new CacheMemoryOptions());

		Task.Factory.StartNew(async () => {
			while (true) {
				c.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "1", TimeSpan.FromSeconds(5));
				await Task.Delay(1000);
			}
		});

		Console.ReadKey();
	}
}
