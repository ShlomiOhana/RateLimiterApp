namespace RateLimiterApp.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWithinWindow(this DateTime timestamp, DateTime startTime, TimeSpan timeWindow)
        {
            return timestamp >= startTime && timestamp <= startTime + timeWindow;
        }

        public static DateTime RoundToMinute(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
        }
    }
}
