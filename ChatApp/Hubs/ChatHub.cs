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
            string[] greetings = new [] { $"{user} is here.", $"{user} has joined! Say hi.", $"Welcome, {user}." };
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

        public async Task SendMessage(UserMessage message) 
        {
            UserConnection user = GetUser();
            if (user != null)
            {
                await Clients.Group(user.Room).SendAsync("ReceiveMessage", user.User, message.Message);

                if (message.Message == "!toastfact")
                {
                    await SendBotMessage();
                }
            }
        }

        public async Task SendBotMessage()
        {
            UserConnection user = GetUser();
            if (user != null)
            {
                await Clients.Group(user.Room).SendAsync("ReceiveMessage", BotUser, GenerateRandomToastFact());
            }
        }

        public UserConnection GetUser()
        {
            if (Connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                return userConnection;
            } else
            {
                return null;
            }
        }

        public async Task LeaveRoomAsync(UserConnection userConnection)
        {
            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", BotUser, $"{userConnection.User} has left the chatroom.");
            Connections.Remove(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.Room);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task<List<UserConnection>> GetUsersForRoom(string room)
        {
            return Task.FromResult(Connections.Values.Where(c => c.Room == room).ToList());
        }
    }
}
