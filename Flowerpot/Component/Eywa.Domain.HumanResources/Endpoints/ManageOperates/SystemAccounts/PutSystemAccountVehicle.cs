namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.SystemAccounts;
internal sealed class PutSystemAccountVehicle : NodeEnlarge<PutSystemAccountVehicle, PutSystemAccountInput>
{
    public override void Configure()
    {
        AllowFormData(urlEncoded: true);
        RequiredConfiguration();
    }
    public override async Task HandleAsync(PutSystemAccountInput req, CancellationToken ct)
    {
        VerifyRootMember();
        await Validator.LeachAsync(req).ConfigureAwait(false);
        if (!File.Exists(CiphertextPolicy.RootCodePath)) throw new Exception(LocalCulture.Link(HumanResourcesFlag.SystemAccountIsRequired));
        await CiphertextPolicy.PushRootCodeAsync(req.Password).ConfigureAwait(false);
    }
    public required IValidator<PutSystemAccountInput> Validator { get; init; }
}