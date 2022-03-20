using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string BotUser;
        private readonly IDictionary<string, UserConnection> Connections;

        public ChatHub(IDictionary<string, UserConnection> connections)
        {
            BotUser = "ToastBot";
            Connections = connections;
        }

        public string GenerateRandomGreeting(string user)
        {
            Random random = new Random();
            string[] greetings = new [] { $"{user} is here!", $"{user} has joined! Say hi.", $"Welcome, {user}! Party time!" };
            return greetings[random.Next(greetings.Length)];
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", BotUser, GenerateRandomGreeting(userConnection.User));
        }

        public async Task SendMessage(UserMessage message) 
        {
            if (Connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message);
            }
        }
    }
}
