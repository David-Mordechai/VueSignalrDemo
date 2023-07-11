using Microsoft.AspNetCore.SignalR;

namespace Backend;

public class PayloadHub : Hub
{
    private readonly ILogger<PayloadHub> _logger;

    public PayloadHub(ILogger<PayloadHub> logger)
    {
        _logger = logger;
    }
    
    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected");
        return base.OnDisconnectedAsync(exception);
    }

    public void SendCommand(string commandType)
    {
        _logger.LogInformation("Command {Command} invoked", commandType);
        Clients.All.SendAsync("receiveReport", RandomColor(), commandType);
    }

    private static string RandomColor()
    {
        string[] colors = { "red", "green", "blue", "yellow", "orange" };
        return colors[Random.Shared.Next(colors.Length)];
    }
}