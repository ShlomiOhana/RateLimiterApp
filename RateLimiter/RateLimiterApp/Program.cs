using RateLimiterApp.Models;
using RateLimiterApp.Services;

namespace RateLimiterApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var rateLimits = new List<RateLimit>
            {
                new RateLimit(10, TimeSpan.FromSeconds(30)), // 10 requests per 30 seconds
                new RateLimit(50, TimeSpan.FromMinutes(1)), // 50 requests per minute
                new RateLimit(100, TimeSpan.FromHours(1)), // 100 requests per hour
            };

            async Task PerformApiCall(string arg)
            {
                Console.WriteLine($"Making API call with argument: {arg} at {DateTime.UtcNow}");
                await Task.Delay(500); // Demo of some api operation
            }

            var rateLimiter = new RateLimiterService<string>(rateLimits, PerformApiCall);
            for (int i = 0; i < 20; i++)
            {
                await rateLimiter.Perform(i.ToString());
            }
        }
    }
}
