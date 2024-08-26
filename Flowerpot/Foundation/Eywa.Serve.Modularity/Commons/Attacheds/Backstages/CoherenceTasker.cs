namespace Eywa.Serve.Modularity.Commons.Attacheds.Backstages;
internal sealed class CoherenceTasker : HostedService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => KeepAsync(this, async () =>
    {
        await ConfigurePicker.GetConfigurationAsync<CoherenceTasker>(ct: stoppingToken).ConfigureAwait(false);
        foreach (var modular in InscribedExpand.Modulars)
        {
            foreach (var type in modular.Value.GetAssemblyTypes<NpgsqlBase>())
            {
                await NpgsqlHelper.CreateAsync(type).ConfigureAwait(false);
                InscribedExpand.RemoveModule(modular.Value);
            }
            foreach (var type in modular.Value.GetAssemblyTypes<SQLiteBase>())
            {
                await SQLiteHelper.CreateAsync(type).ConfigureAwait(false);
                InscribedExpand.RemoveModule(modular.Value);
            }
        }
    });
    public required INpgsqlHelper NpgsqlHelper { get; init; }
    public required ISQLiteHelper SQLiteHelper { get; init; }
    public required IConfigurePicker ConfigurePicker { get; init; }
}