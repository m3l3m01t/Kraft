using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kraft.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

#if HAVE_REDIS
using StackExchange.Redis;
using static StackExchange.Redis.RedisChannel;
#endif

namespace Kraft.Server.Services
{
    public class HeartBeator : IHostedService
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly Random _rand;
        private readonly Hubs.Beacon _notifier;

#if HAVE_REDIS
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly ISubscriber _subscriber;

        public HeartBeator(IOptions<AppSettings> appSettings,
                        IConnectionMultiplexer multiplexer)
        {
            _tokenSource = new CancellationTokenSource();

            _multiplexer = multiplexer;
            _subscriber = multiplexer.GetSubscriber();

        }
#else
        public HeartBeator(IOptions<AppSettings> appSettings, Hubs.Beacon notifier)
        {
            _notifier = notifier;
            _tokenSource = new CancellationTokenSource();

            _rand = new Random();
        }
#endif

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Observable
                .Interval(TimeSpan.FromMilliseconds(1000))
                .SubscribeOn(ThreadPoolScheduler.Instance)
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(async t =>
                {
#if HAVE_REDIS
                    _subscriber.Publish(new RedisChannel("heartbeat", PatternMode.Auto), $"beating {t}");
#else
                    var strength = _rand.Next(0, 100);
                    var beat = new Beat { Beats = t, TimeStamp = DateTime.Now, Strength = strength };

                    await _notifier.NotifyAsync(beat);
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