using System;
using System.Threading;
using System.Threading.Tasks;
using Kraft.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#if HAVE_REDIS

using StackExchange.Redis;

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
        private readonly ILogger<HeartBeator> _logger;
        private readonly ISubscriber _subscriber;
        private int _beats = 0;

        public HeartBeator(IOptions<AppSettings> appSettings,
                        IConnectionMultiplexer multiplexer
                        , Hubs.Beacon notifier,
                        ILogger<HeartBeator> logger)
        {
            _tokenSource = new CancellationTokenSource();
            _rand = new Random();

            _multiplexer = multiplexer;
            _notifier = notifier;
            _logger = logger;

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
#if HAVE_REDIS
            return _subscriber.SubscribeAsync(new RedisChannel("heartbeat", RedisChannel.PatternMode.Auto), async (channel, msg) =>
            {
                _logger.LogDebug($"Redising {msg.ToString()}");

                var strength = _rand.Next(0, 100);

                var beat = new Beat
                {
                    Beats = ++_beats,
                    TimeStamp = DateTime.Now,
                    Strength = strength,
                    Payload = msg.ToString()
                };

                await _notifier.NotifyAsync(beat);
            });
#else
            Observable
                .Interval(TimeSpan.FromMilliseconds(1000))
                .SubscribeOn(ThreadPoolScheduler.Instance)
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(async t =>
                {
                    var strength = _rand.Next(0, 100);
                    var beat = new Beat { Beats = t, TimeStamp = DateTime.Now, Strength = strength };

                    await _notifier.NotifyAsync(beat);
                }, _tokenSource.Token);

            return Task.CompletedTask;
#endif
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();

            return Task.CompletedTask;
        }
    }
}