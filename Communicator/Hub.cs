using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator
{
    public class Hub
    {
        private HubConnection hubConnection;

        public Hub()
        {
            hubConnection = new HubConnectionBuilder()
                //.WithUrl("http://localhost:5043")
                .WithUrl("https://192.168.1.128:7174")
                .Build();
            hubConnection.On<string>("ReceiveMessage", (message) =>
            {
                //chatMessages.Text += $"{Environment.NewLine}{message}";
            });
            //hubConnection.Closed += async (error) =>
            //{
            //    await Task.Delay(new Random().Next(0, 5) * 1000);
            //    await hubConnection.StartAsync();
            //};
        }

        public Task Connect()
        {
            return hubConnection.StartAsync();
        }

        public Task Disconnect()
        {
            return hubConnection.StopAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            var a = hubConnection.ConnectionId;
            var b = hubConnection.State;
            await hubConnection.InvokeAsync("SendMessage", user, message);
        }
    }
}
