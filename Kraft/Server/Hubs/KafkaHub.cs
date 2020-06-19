﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Kraft.Server.Hubs
{
    public class KafkaHub : Hub
    {
        private readonly ILogger<KafkaHub> _logger;

        public KafkaHub(ILogger<KafkaHub> logger)
        {
            _logger = logger;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        
        public async Task Leave(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
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