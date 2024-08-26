namespace Eywa.Serve.Constructs.Grindstones.Composers;
public abstract class PlatformBuilder
{
    public abstract PlatformBuilder Build<T>() where T : BaseModule;
    public abstract PlatformBuilder Create(in Action<WebApplicationBuilder>? builder = default);
    public abstract ValueTask<PlatformBuilder> RunAsync(Action<WebApplication, IEndpointRouteBuilder>? options = default);
    public WebApplication? WebApp { get; protected set; }
}