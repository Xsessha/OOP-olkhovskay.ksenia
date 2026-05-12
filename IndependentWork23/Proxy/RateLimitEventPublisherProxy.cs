using System;
using System.Collections.Generic;

namespace IndependentWork23.Proxy
{
    public class RateLimitEventPublisherProxy : IEventPublisher
    {
        private readonly RealEventPublisher _realPublisher;
        private readonly Dictionary<string, CachedPublication> _cache;
        private DateTime _lastPublishTime;
        private readonly int _delaySeconds;
        private readonly TimeSpan _cacheDuration;

        public RateLimitEventPublisherProxy(int delaySeconds, int cacheSeconds = 5)
        {
            _realPublisher = new RealEventPublisher();
            _delaySeconds = delaySeconds;
            _cacheDuration = TimeSpan.FromSeconds(cacheSeconds);
            _lastPublishTime = DateTime.MinValue;
            _cache = new Dictionary<string, CachedPublication>();
        }

        public string Publish(string eventName)
        {
            if (_cache.TryGetValue(eventName, out CachedPublication? cached) &&
                cached is not null && DateTime.Now - cached.Timestamp <= _cacheDuration)
            {
                string cacheMessage = $"[PROXY CACHE] Returning cached publication for event: {eventName}";
                Console.WriteLine(cacheMessage);
                return cached.Result;
            }

            TimeSpan timePassed = DateTime.Now - _lastPublishTime;
            if (timePassed.TotalSeconds < _delaySeconds)
            {
                string blockedMessage = $"[PROXY BLOCKED] Too many requests. Wait {_delaySeconds} seconds.";
                Console.WriteLine(blockedMessage);
                return blockedMessage;
            }

            string result = _realPublisher.Publish(eventName);
            _lastPublishTime = DateTime.Now;
            _cache[eventName] = new CachedPublication(result, DateTime.Now);
            return result;
        }

        private sealed class CachedPublication
        {
            public string Result { get; }
            public DateTime Timestamp { get; }

            public CachedPublication(string result, DateTime timestamp)
            {
                Result = result;
                Timestamp = timestamp;
            }
        }
    }
}