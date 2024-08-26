namespace Eywa.Serve.Constructs.Grindstones.Substances;
public sealed class StandardPlatform(IEditionRepository editionRepository) : PlatformBuilder
{
    public override PlatformBuilder Build<T>()
    {
        WebBuilder = editionRepository.Initialize(Assembly.GetAssembly(typeof(T)));
        return this;
    }
    public override PlatformBuilder Create(in Action<WebApplicationBuilder>? builder = default)
    {
        editionRepository.Add(WebBuilder, x =>
        {
            List<InternalExpand.ListenInfo> listens = [];
            var info = WebBuilder.Configuration.GetSection(nameof(HostInformation)).Get<HostInformation>();
            if (info is not null)
            {
                if (info.HTTPS.Enabled)
                {
                    x.ConfigureHttpsDefaults(x => x.SslProtocols = SslProtocols.Tls12);
                    listens.Add(new(nameof(HostInformation.HTTPS), info.HTTPS.Port, x =>
                    {
                        x.UseHttps(info.HTTPS.Certificate.Location, info.HTTPS.Certificate.Password);
                    }));
                }
                listens.Add(new(nameof(HostInformation.HTTP), info.HTTP.Port, default));
            }
            if (listens.Count is not 0) x.Listening(listens);
        });
        if (builder is not null) builder(WebBuilder);
        return this;
    }
    public override async ValueTask<PlatformBuilder> RunAsync(Action<WebApplication, IEndpointRouteBuilder>? options = default)
    {
        WebApp = editionRepository.Add(WebBuilder, (app, endpoint) =>
        {
            endpoint.MapControllers();
            endpoint.MapFastEndpoints(x =>
            {
                x.Serializer.Options.PropertyNameCaseInsensitive = true;
            });
            if (options is not null) options(app, endpoint);
        });
        var info = WebApp.Configuration.GetSection(nameof(HostInformation)).Get<HostInformation>();
        await InternalExpand.CloseMarqueeAsync().ConfigureAwait(false);
        if (info is not null)
        {
            await info.Identifier.PrintAsync(ConsoleColor.DarkGreen, newline: true).ConfigureAwait(false);
            await string.Empty.PrintAsync().ConfigureAwait(false);
        }
        if (InternalExpand.ListenBanners.Count is not (int)default)
        {
            await string.Join(Environment.NewLine, InternalExpand.ListenBanners).PrintAsync(ConsoleColor.Blue).ConfigureAwait(false);
        }
        await string.Empty.PrintAsync(ConsoleColor.White).ConfigureAwait(false);
        return this;
    }
    WebApplicationBuilder WebBuilder { get; set; } = null!;
}