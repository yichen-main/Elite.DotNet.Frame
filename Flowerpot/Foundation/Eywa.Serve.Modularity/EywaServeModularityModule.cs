namespace Eywa.Serve.Modularity;

[DependsOn(typeof(PrehistoricModule))]
public sealed class EywaServeModularityModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Initialize(this, builder);
        builder.RegisterBuildCallback(x =>
        {
            var configuration = x.Resolve<IConfiguration>();
            x.Resolve<IConfigurePicker>().InitialProfile<EywaServeModularityModule>();
            x.Resolve<INpgsqlHelper>().SetConfiguration(configuration.GetConnectionString(nameof(Npgsql)));
        });
    }
}