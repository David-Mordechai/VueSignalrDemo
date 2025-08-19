using Microsoft.AspNetCore.SignalR;

namespace Backend;

public class PayloadHub(ILogger<PayloadHub> logger) : Hub
{
    public override Task OnConnectedAsync()
    {
        logger.LogInformation("Client connected");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Client disconnected");
        return base.OnDisconnectedAsync(exception);
    }

    public void SendCommand(string commandType)
    {
        logger.LogInformation("Command {Command} invoked", commandType);
        Clients.All.SendAsync("receiveReport", RandomColor(), commandType);
    }

    private static string RandomColor()
    {
        string[] colors = ["red", "green", "blue", "yellow", "orange"];
        return colors[Random.Shared.Next(colors.Length)];
    }
}