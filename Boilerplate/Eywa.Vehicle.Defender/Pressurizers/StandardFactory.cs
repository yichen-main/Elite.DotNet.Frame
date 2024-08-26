namespace Eywa.Vehicle.Defender.Pressurizers;

[Modular]
internal sealed class StandardFactory(StandardPlatform platform) : BattleshipBuilder
{
    public override BattleshipBuilder Add()
    {
        platform.Build<AppModule>();
        platform.Create(x =>
        {
            x.Services.AddGraphQLServer()
            .AddQueryType<OneselfQueryer>()
            .AddMutationType<OneselfMutator>().AddMutationConventions()
            .AddSubscriptionType<OneselfSubscriber>().AddInMemorySubscriptions();
        });
        return this;
    }
    public override async ValueTask<BattleshipBuilder> RunAsync()
    {
        await platform.RunAsync((app, endpoint) =>
        {
            var hostInfo = app.Configuration.GetSection(nameof(HostInformation)).Get<HostInformation>();
            if (hostInfo is not null)
            {
                endpoint.MapGraphQL(hostInfo.HTTP.Path.GraphQL);
            }
        }).ConfigureAwait(false);
        return this;
    }
}