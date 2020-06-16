using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using static StackExchange.Redis.RedisChannel;

namespace Kraft.Server.Services
{
    public class HeartBeator : IHostedService
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly Hubs.Beacon _notifier;

#if true
        public HeartBeator(IOptions<AppSettings> appSettings, Hubs.Beacon notifier)
        {
            this._notifier = notifier;
            _tokenSource = new CancellationTokenSource();

        }
#else
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly ISubscriber _subscriber;

        public HeartBeator(IOptions<AppSettings> appSettings,
                        IConnectionMultiplexer multiplexer)
        {
            _tokenSource = new CancellationTokenSource();

            _multiplexer = multiplexer;
            _subscriber = multiplexer.GetSubscriber();
        }
#endif

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Observable
                .Interval(TimeSpan.FromSeconds(5))
                .SubscribeOn(ThreadPoolScheduler.Instance)
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(t =>
                {
#if false
                    _subscriber.Publish(new RedisChannel("heartbeat", PatternMode.Auto), $"beating {t}");
#else
                    EventHandler handler = _notifier.HeatBeat;

                    handler?.Invoke(this, new EventArgs() );
#endif
                }, _tokenSource.Token);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();

            return Task.CompletedTask;
        }
    }
}