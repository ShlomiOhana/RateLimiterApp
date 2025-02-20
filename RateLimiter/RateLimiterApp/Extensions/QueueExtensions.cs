namespace RateLimiterApp.Extensions
{
    public static class QueueExtensions
    {
        public static void CleanQueue(this Queue<DateTime> queue, DateTime thresholdTime)
        {
            while (queue.Count > 0 && queue.Peek() < thresholdTime)
            {
                queue.Dequeue();
            }
        }
    }
}
