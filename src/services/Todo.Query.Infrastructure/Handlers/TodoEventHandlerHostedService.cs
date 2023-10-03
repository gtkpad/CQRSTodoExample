using Microsoft.Extensions.Hosting;

namespace Todo.Query.Infrastructure.Handlers;

public class TodoEventHandlerHostedService : IHostedService
{
    private readonly ITodoEventHandler _handler;

    public TodoEventHandlerHostedService(ITodoEventHandler handler)
    {
        _handler = handler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _handler.Subscribe();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stoping");
    }
}