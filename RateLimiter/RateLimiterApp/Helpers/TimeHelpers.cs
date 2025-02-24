using System.Collections.Concurrent;

namespace RateLimiterApp.Helpers
{
    public static class TimeHelpers
    {
        public static TimeSpan CalculateDelay(ConcurrentQueue<DateTime> queue, DateTime thresholdTime, TimeSpan timeWindow)
        {
            if (queue.Count == 0)
                return TimeSpan.Zero;

            DateTime result;
            queue.TryPeek(out result);
            DateTime nextAvailableTime = result.Add(timeWindow);
            return nextAvailableTime - DateTime.UtcNow;
        }
    }
}