using RateLimiterApp.Extensions;
using RateLimiterApp.Helpers;
using RateLimiterApp.Models;

namespace RateLimiterApp.Services
{
    public class RateLimiterService<TArg>
    {
        private readonly List<RateLimit> _rateLimits;
        private readonly List<Queue<DateTime>> _requestQueues;
        private readonly SemaphoreSlim _semaphore;
        private readonly Func<TArg, Task> _func;

        public RateLimiterService(IEnumerable<RateLimit> rateLimits, Func<TArg, Task> func)
        {
            _rateLimits = rateLimits.ToList();
            _requestQueues = new List<Queue<DateTime>>(_rateLimits.Count);

            foreach (var _ in rateLimits)
            {
                _requestQueues.Add(new Queue<DateTime>());
            }

            _func = func;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task Perform(TArg argument)
        {
            await _semaphore.WaitAsync();
            try
            {
                for (int i = 0; i < _rateLimits.Count; i++)
                {
                    var queue = _requestQueues[i];
                    DateTime thresholdTime = DateTime.UtcNow - _rateLimits[i].TimeWindow;

                    queue.CleanQueue(thresholdTime);

                    if (queue.Count >= _rateLimits[i].MaxRequests)
                    {
                        var delay = TimeHelpers.CalculateDelay(queue, thresholdTime, _rateLimits[i].TimeWindow);
                        if (delay > TimeSpan.Zero)
                        {
                            await Task.Delay(delay);
                        }
                    }
                    queue.Enqueue(DateTime.UtcNow);
                }

                await _func(argument);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
