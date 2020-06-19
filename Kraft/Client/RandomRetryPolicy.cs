using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Kraft.Client
{
    public class RandomRetryPolicy : IRetryPolicy
    {
        private readonly Random _rand;

        public RandomRetryPolicy()
        {
            _rand = new Random();
        }
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            return TimeSpan.FromSeconds(_rand.Next(1, 4));
        }
    }
}