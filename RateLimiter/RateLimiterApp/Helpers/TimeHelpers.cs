namespace RateLimiterApp.Helpers
{
    public static class TimeHelpers
    {
        public static TimeSpan CalculateDelay(Queue<DateTime> queue, DateTime thresholdTime, TimeSpan timeWindow)
        {
            if (queue.Count == 0)
                return TimeSpan.Zero;

            DateTime nextAvailableTime = queue.Peek().Add(timeWindow);
            return nextAvailableTime - DateTime.UtcNow;
        }
    }
}