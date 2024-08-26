namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountSignIn;
internal sealed class AddAccountSignInVehicle : NodeEnlarge<AddAccountSignInVehicle, AddAccountSignInInput>
{
    public override void Configure()
    {
        AllowAnonymous();
        AllowFormData(urlEncoded: true);
        RequiredConfiguration();
    }
    public override Task HandleAsync(AddAccountSignInInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            var refreshToken = CiphertextPolicy.GetRefreshToken();
            var rootCodeAsync = CiphertextPolicy.PullRootCodeAsync();
            await Validator.LeachAsync(req).ConfigureAwait(false);
            OutputToken output = default;
            switch (req.Account)
            {
                case var item when item.IsMatch(ConfigurePicker.ModuleProfile.HTTPAuthentication.AccountName):
                    {
                        output = GetTokenInfo(new()
                        {
                            Id = req.Account,
                            UserName = req.Account,
                        }, refreshToken);
                        var rootCode = await rootCodeAsync.ConfigureAwait(false);
                        if (!rootCode.IsMatch(CiphertextPolicy.HashSHA256(req.Password)))
                            throw new Exception(LocalCulture.Link(HumanResourcesFlag.WrongPassword));
                    }
                    break;

                default:
                    var reader = await readerAsync.ConfigureAwait(false);
                    var user = await reader.ReadFirstOrDefaultAsync<HumanUser>().ConfigureAwait(false)
                    ?? throw new Exception(LocalCulture.Link(HumanResourcesFlag.SignInFailed));
                    if (user is { Disable: true }) throw new Exception(LocalCulture.Link(HumanResourcesFlag.AccountInvalid));
                    {
                        var hashedText = CiphertextPolicy.HmacSHA256ToHex(req.Password, user.Salt);
                        if (!user.HashedText.IsMatch(hashedText)) throw new Exception(LocalCulture.Link(HumanResourcesFlag.SignInFailed));
                        var (sql, param) = TableLayout.LetInsert(new HumanUserRecord
                        {
                            Id = TableLayout.GetSnowflakeId(),
                            UserId = user.Id,
                            LoginStatus = LoginStatus.SignIn,
                        });
                        output = GetTokenInfo(new()
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                        }, refreshToken);
                        await connection.ExecuteAsync(sql, param).ConfigureAwait(false);
                    }
                    break;
            }
            SetRefreshTokenCookies(refreshToken);
            await SendAsync(output, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(nameof(HumanUser.Email), req.Account),
        ]);
    }
    public required IValidator<AddAccountSignInInput> Validator { get; init; }
}