using System;

namespace EasyCacheNET {

	public static class Utility {
		public static DateTimeOffset IncrementDate(TimeSpan duration) {
			// TODO: Safe return.
			return DateTimeOffset.UtcNow.AddSeconds(duration.TotalSeconds);
		}
	}

}
