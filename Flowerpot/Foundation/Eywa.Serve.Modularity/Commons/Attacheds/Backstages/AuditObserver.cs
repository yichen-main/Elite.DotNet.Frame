namespace Eywa.Serve.Modularity.Commons.Attacheds.Backstages;
internal sealed class AuditObserver : HostedService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => ExecuteAsync(this, ProfileExpand.KeepLogAsync);
}