namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.SystemAccounts;
internal sealed class AddSystemAccountVehicle : NodeEnlarge<AddSystemAccountVehicle, AddSystemAccountInput>
{
    public override void Configure()
    {
        AllowAnonymous();
        AllowFormData(urlEncoded: true);
        RequiredConfiguration();
    }
    public override async Task HandleAsync(AddSystemAccountInput req, CancellationToken ct)
    {
        await Validator.LeachAsync(req).ConfigureAwait(false);
        if (File.Exists(CiphertextPolicy.RootCodePath)) throw new Exception(LocalCulture.Link(HumanResourcesFlag.SystemAccountAlreadyExists));
        await CiphertextPolicy.PushRootCodeAsync(req.Password).ConfigureAwait(false);
        await SendCreatedAtAsync<ListSystemAccountVehicle>(new(), default, cancellation: ct).ConfigureAwait(false);
    }
    public required IValidator<AddSystemAccountInput> Validator { get; init; }
}