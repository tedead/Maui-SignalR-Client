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

        try
        {
            Connect();
        }
        catch(Exception ex) 
        {
            Debug.WriteLine(string.Format("Error connecting: {0}", ex.Message));
        }
    }

    private void ConnectClicked(object sender, EventArgs e)
	{

		SemanticScreenReader.Announce("Connected");
	}

    private void SendClicked(object sender, EventArgs e)
    {
        _ = SendMessage();
    }

    //public Task Connect()
    //{
    //    return hubConnection.StartAsync();
    //}

    public Task Disconnect()
    {
        return hubConnection.StopAsync();
    }

    public async Task SendMessage()
    {

        if(hubConnection == null || hubConnection.ConnectionId == null)
        {
            try
            {
                Connect();
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(string.Format("Error connecting: {0}", ex.Message));
            }
        }

        await hubConnection.InvokeAsync("SendMessage", name.Text, message.Text);
    }

    public void Connect()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("http://192.168.1.128:5043/chatHub")
            //.WithUrl("https://192.168.1.128:7174")
            .Build();
        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Debug.WriteLine(user, message);
            messageList.Text += $"{message}";
        });

        Task.Run(() => { Dispatcher.Dispatch(async () => await hubConnection.StartAsync()); });
    }
}

