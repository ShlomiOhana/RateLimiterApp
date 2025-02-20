namespace RateLimiterApp.Models
{
    public class RateLimit
    {
        public int MaxRequests { get; }
        public TimeSpan TimeWindow { get; }

        public RateLimit(int maxRequests, TimeSpan timeWindow)
        {
            MaxRequests = maxRequests;
            TimeWindow = timeWindow;
        }
    }
}