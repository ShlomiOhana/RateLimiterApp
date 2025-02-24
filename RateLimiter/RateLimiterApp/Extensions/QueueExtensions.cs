using System.Collections.Concurrent;

namespace RateLimiterApp.Extensions
{
    public static class QueueExtensions
    {
        public static void CleanQueue(this ConcurrentQueue<DateTime> queue, DateTime thresholdTime)
        {
            DateTime peeked;
            while (queue.TryPeek(out peeked) && peeked < thresholdTime)
            {
                queue.TryDequeue(out _);
            }
        }
    }
}
