namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.SystemAccounts;
internal sealed class ListSystemAccountVehicle : NodeEnlarge<ListSystemAccountVehicle, NodeUnique>
{
    public override void Configure()
    {
        AllowAnonymous();
        RequiredConfiguration();
    }
    public override async Task HandleAsync(NodeUnique req, CancellationToken ct)
    {
        var isExistAccount = false;
        if (File.Exists(CiphertextPolicy.RootCodePath)) isExistAccount = true;
        await SendAsync(new Output
        {
            IsExistAccount = isExistAccount,
            AccountName = ConfigurePicker.ModuleProfile.HTTPAuthentication.AccountName,
        }, cancellation: ct).ConfigureAwait(false);
    }
    public readonly struct Output
    {
        public required bool IsExistAccount { get; init; }
        public required string AccountName { get; init; }
    }
}