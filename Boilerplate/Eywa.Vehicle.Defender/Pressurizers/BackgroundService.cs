namespace Eywa.Vehicle.Defender.Pressurizers;
public sealed class BackgroundService : HostedService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //IMyService proxy = LoggingProxy<IMyService>.Create(MyProperty);

        //proxy.DoSomething("test");
        //proxy.DoSomethingElse(42);
        return Task.CompletedTask;
    }
    public required IMyService MyProperty { get; init; }
}