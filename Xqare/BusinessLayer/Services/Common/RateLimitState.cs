using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Xqare.BusinessLayer.Services.Common
{
    public sealed class RateLimitState : IDisposable
    {
        private readonly object _sync = new();
        private readonly Dictionary<string, DateTimeOffset> _blockedUntil =
            new(StringComparer.OrdinalIgnoreCase);
        private readonly Timer _timer;

        public event Action? Changed;

        public RateLimitState()
        {
            _timer = new Timer(_ => Tick(), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public bool IsBlocked(string? policy) => GetRemainingSeconds(policy) > 0;

        public int GetRemainingSeconds(string? policy)
        {
            if (string.IsNullOrWhiteSpace(policy))
                return 0;

            lock (_sync)
            {
                if (!_blockedUntil.TryGetValue(policy, out var until))
                    return 0;

                var remaining = (int)Math.Ceiling((until - DateTimeOffset.UtcNow).TotalSeconds);
                if (remaining <= 0)
                {
                    _blockedUntil.Remove(policy);
                    return 0;
                }

                return remaining;
            }
        }

        public void SetBlocked(string policy, int retryAfterSeconds)
        {
            if (string.IsNullOrWhiteSpace(policy) || retryAfterSeconds <= 0)
                return;

            lock (_sync)
            {
                var until = DateTimeOffset.UtcNow.AddSeconds(Math.Clamp(retryAfterSeconds, 1, 86400));
                if (!_blockedUntil.TryGetValue(policy, out var existing) || until > existing)
                    _blockedUntil[policy] = until;
            }

            Changed?.Invoke();
        }

        private void Tick()
        {
            bool hasActiveCountdown;
            lock (_sync)
            {
                foreach (var key in _blockedUntil
                    .Where(pair => pair.Value <= DateTimeOffset.UtcNow)
                    .Select(pair => pair.Key)
                    .ToArray())
                {
                    _blockedUntil.Remove(key);
                }

                hasActiveCountdown = _blockedUntil.Count > 0;
            }

            // Notify while a countdown is active so notices and disabled buttons
            // update once per second. All dictionary access remains synchronized.
            if (hasActiveCountdown)
                Changed?.Invoke();
        }

        public void Dispose() => _timer.Dispose();
    }
}