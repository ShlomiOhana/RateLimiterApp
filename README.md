**Sliding Window**

**Pros:**
- Offers requests distribution more evenly over time than the second approach.
- Reduces burstiness of requests (many requests in a short period of time) compared to the absolute approach.

**Cons:**
- Slightly more complex to implement than the absolute approach.
- Requires maintaining and frequently checking timestamps for each rate limit.
