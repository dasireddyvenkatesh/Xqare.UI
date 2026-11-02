using System.Timers;
using Timer = System.Timers.Timer;

namespace Xqare.BusinessLayer.Classes
{
    public class VisitCounterService : IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Timer _timer;
        private int _total;
        private int _hourly;
        public event Action? OnChange;
        public VisitCounterService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _timer = new Timer(TimeSpan.FromHours(1).TotalMilliseconds);
            _timer.Elapsed += SaveToDatabase;
            _timer.Start();
        }
        public void Increment()
        {
            Interlocked.Increment(ref _total);
            Interlocked.Increment(ref _hourly);
            OnChange?.Invoke();
        }
        public int Total => _total;
        public int ThisHour => _hourly;
        private async void SaveToDatabase(object? sender, ElapsedEventArgs e)
        {
            var count = Interlocked.Exchange(ref _hourly, 0);
            if (count == 0)
                return;
            //using var scope = _scopeFactory.CreateScope();
            //var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //db.PageVisits.Add(new PageVisit
            //{
            //    Count = count,
            //    CreatedAt = DateTime.UtcNow
            //});
            //await db.SaveChangesAsync();
        }
        public void Dispose() => _timer.Dispose();
    }
}
