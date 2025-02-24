using RateLimiterApp.Extensions;
using RateLimiterApp.Helpers;
using RateLimiterApp.Models;
using System.Collections.Concurrent;

namespace RateLimiterApp.Services
{
    public class RateLimiterService<TArg>
    {
        private readonly List<RateLimit> _rateLimits;
        private readonly List<ConcurrentQueue<DateTime>> _requestQueues;
        private readonly SemaphoreSlim _semaphore;
        private readonly Func<TArg, Task> _func;

        public RateLimiterService(IEnumerable<RateLimit> rateLimits, Func<TArg, Task> func)
        {
            _rateLimits = rateLimits.ToList();
            _requestQueues = new List<ConcurrentQueue<DateTime>>(_rateLimits.Count);

            foreach (var _ in rateLimits)
            {
                _requestQueues.Add(new ConcurrentQueue<DateTime>());
            }

            _func = func;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task Perform(TArg argument)
        {
            var delayedTasks = new List<Task>();

            for (int i = 0; i < _rateLimits.Count; i++)
            {
                await _semaphore.WaitAsync();
                try
                {
                    var queue = _requestQueues[i];
                    DateTime thresholdTime = DateTime.UtcNow - _rateLimits[i].TimeWindow;

                    queue.CleanQueue(thresholdTime);

                    if (queue.Count >= _rateLimits[i].MaxRequests)
                    {
                        var delay = TimeHelpers.CalculateDelay(queue, thresholdTime, _rateLimits[i].TimeWindow);

                        if (delay > TimeSpan.Zero)
                        {
                            delayedTasks.Add(Task.Delay(delay));
                        }
                    }

                    queue.Enqueue(DateTime.UtcNow);
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            if (delayedTasks.Any())
            {
                await Task.WhenAll(delayedTasks);
            }

            await _func(argument);
        }



    }
}
