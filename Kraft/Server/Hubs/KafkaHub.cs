using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Kraft.Server.Hubs
{
    public class KafkaHub : Hub
    {
        private readonly ILogger<KafkaHub> _logger;
        private readonly Beacon _notifier;

        public KafkaHub(ILogger<KafkaHub> logger, Beacon notifier)
        {
            this._notifier = notifier;
            _logger = logger;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            _logger.LogInformation("{0}: {1} connected", Context.ConnectionId, Context.UserIdentifier);

            return Clients.Client(Context.ConnectionId).SendAsync("Welcome", Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogError(exception, "{0}: {1} disconnected", Context.ConnectionId, Context.UserIdentifier);

            return base.OnDisconnectedAsync(exception);
        }
    }
}