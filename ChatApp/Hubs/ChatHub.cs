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
        private List<string> toastFacts;
        private Random random;
        public ChatHub(IDictionary<string, UserConnection> connections)
        {
            BotUser = "ToastBot";
            Connections = connections;
            toastFacts = new List<string>()
            {
                "toast is so yummy",
                "toast is fun to eat",
                "i love toast",
                "please feed me toast",
                "toast is just bread",
                "something else"
            };
            random = new Random();
        }

        public string GenerateRandomGreeting(string user)
        {
            string[] greetings = new [] { $"{user} is here!", $"{user} has joined! Say hi.", $"Welcome, {user}! Party time!" };
            return greetings[random.Next(greetings.Length)];
        }

        public string GenerateRandomToastFact()
        {
            return toastFacts[random.Next(toastFacts.Count)];
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            Connections[Context.ConnectionId] = userConnection;
            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", BotUser, GenerateRandomGreeting(userConnection.User));
        }

        public async Task SendMessage(string message) 
        {
            if (Connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message);

                if (message == "!toastfact")
                {
                    await SendBotMessage(userConnection);
                }
            }
        }

        public async Task SendBotMessage(UserConnection userConnection)
        {
            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", BotUser, GenerateRandomToastFact());
        }
    }
}
