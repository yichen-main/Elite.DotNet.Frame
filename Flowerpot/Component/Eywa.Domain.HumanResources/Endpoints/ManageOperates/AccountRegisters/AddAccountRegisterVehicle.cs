namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountRegisters;
internal sealed class AddAccountRegisterVehicle : NodeEnlarge<AddAccountRegisterVehicle, AddAccountRegisterInput>
{
    public override void Configure()
    {
        AllowFormData(urlEncoded: true);
        RequiredConfiguration();
    }
    public override Task HandleAsync(AddAccountRegisterInput req, CancellationToken ct)
    {
        VerifyRootMember();
        var id = TableLayout.GetSnowflakeId();
        var salt = CiphertextPolicy.GenerateRandomSalt();
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            try
            {
                await Validator.LeachAsync(req).ConfigureAwait(false);
                await NpgsqlHelper.InsertAsync(connection, new HumanUser
                {
                    Id = id,
                    Email = req.Email,
                    UserNo = req.UserNo,
                    UserName = req.UserName,
                    Disable = default,
                    Salt = salt,
                    HashedText = CiphertextPolicy.HmacSHA256ToHex(req.Password, salt),
                }, ct).ConfigureAwait(false);
                await SendCreatedAtAsync<GetAccountRegisterVehicle>(new
                {
                    id,
                }, default, cancellation: ct).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                MakeException(e, [
                    (HumanUser.EmailIndex, LocalCulture.Link(HumanResourcesFlag.EmailIndex)),
                    (HumanUser.UserNoIndex, LocalCulture.Link(HumanResourcesFlag.UserNoIndex)),
                ]);
            }
        });
    }
    public required IValidator<AddAccountRegisterInput> Validator { get; init; }
}