namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.TokenExtensions;
internal sealed class AddTokenExtensionVehicle : NodeEnlarge<AddTokenExtensionVehicle, AddTokenExtensionInput>
{
    public override void Configure()
    {
        AllowAnonymous();
        AllowFormData(urlEncoded: true);
        RequiredConfiguration();
    }
    public override Task HandleAsync(AddTokenExtensionInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            await Validator.LeachAsync(req).ConfigureAwait(false);
            if (!GetCookies(RefreshTokenTag).IsMatch(req.RefreshToken)) throw new Exception(LocalCulture.Link(HumanResourcesFlag.TokenInvalid));
            var refreshToken = CiphertextPolicy.GetRefreshToken();
            OutputToken reault = default;
            switch (req.NameIdentifier)
            {
                case var x when x.IsMatch(ConfigurePicker.ModuleProfile.HTTPAuthentication.AccountName):
                    reault = GetTokenInfo(new()
                    {
                        Id = req.NameIdentifier,
                        UserName = req.NameIdentifier,
                    }, refreshToken);
                    break;

                default:
                    var reader = await readerAsync.ConfigureAwait(false);
                    var user = await reader.ReadFirstOrDefaultAsync<HumanUser>().ConfigureAwait(false)
                    ?? throw new Exception(LocalCulture.Link(HumanResourcesFlag.AccountDoesNotExist));
                    if (user is { Disable: true }) throw new Exception(LocalCulture.Link(HumanResourcesFlag.AccountInvalid));
                    reault = GetTokenInfo(new()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                    }, refreshToken);
                    break;
            }
            SetRefreshTokenCookies(refreshToken);
            await SendAsync(reault, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(nameof(HumanUser.Email), req.NameIdentifier),
        ]);
    }
    public required IValidator<AddTokenExtensionInput> Validator { get; init; }
}