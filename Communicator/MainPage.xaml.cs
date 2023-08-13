using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

namespace Communicator;

public partial class MainPage : ContentPage
{
	int count = 0;
    private HubConnection hubConnection;
    public MainPage()
	{
		InitializeComponent();

        hubConnection = new HubConnectionBuilder()
    .WithUrl("http://192.168.1.128:5043/chatHub")
    //.WithUrl("https://192.168.1.128:7174")
    .Build();
        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Debug.WriteLine(user, message);
            messageList.Text = $"{message}";
        });

        Task.Run(() => { Dispatcher.Dispatch(async () => await hubConnection.StartAsync()); });
    }

    private void ConnectClicked(object sender, EventArgs e)
	{

		SemanticScreenReader.Announce("Connected");
	}

    private void SendClicked(object sender, EventArgs e)
    {
		string msg = message.Text;
		string u = name.Text;
        _ = SendMessage(msg, u);

        SemanticScreenReader.Announce("Sent");
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
        //var a = hubConnection.ConnectionId;
        //var b = hubConnection.State;
        await hubConnection.InvokeAsync("SendMessage", user, message);
    }
}

