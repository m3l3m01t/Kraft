using Kraft.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Kraft.Server.Hubs
{
    public class Beacon
    {
        private readonly IHubContext<KafkaHub> _hubCtx;

        public Beacon(IHubContext<KafkaHub> hubCtx)
        {
            _hubCtx = hubCtx;
        }

        internal Task NotifyAsync(Beat beat)
        {
            return _hubCtx.Clients.All.SendAsync("HeartBeat", beat);
        }
    }
}