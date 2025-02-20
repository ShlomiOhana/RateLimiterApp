Sliding Window

**Pros:**
- Offers more precision and fairness as it distributes requests more evenly over the time window.
- Reduces burstiness of requests compared to the absolute approach.

**Cons:**
- Slightly more complex than the absolute approach.
- Requires maintaining and frequently checking timestamps for each rate limit.
