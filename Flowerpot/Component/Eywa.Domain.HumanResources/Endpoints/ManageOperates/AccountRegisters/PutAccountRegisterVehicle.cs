namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountRegisters;
internal sealed class PutAccountRegisterVehicle : NodeEnlarge<PutAccountRegisterVehicle, PutAccountRegisterInput>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(PutAccountRegisterInput req, CancellationToken ct)
    {
        VerifyRootMember();
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            try
            {
                await Validator.LeachAsync(req).ConfigureAwait(false);
                var reader = await readerAsync.ConfigureAwait(false);
                var user = await reader.ReadFirstOrDefaultAsync<HumanUser>().ConfigureAwait(false)
                ?? throw new Exception(LocalCulture.Link(HumanResourcesFlag.UserIdIsRequired));
                if (user is not null) await NpgsqlHelper.UpdateAsync(user.Id, connection, new HumanUser
                {
                    Email = req.Body.Email,
                    UserNo = req.Body.UserNo,
                    UserName = req.Body.UserName,
                    Disable = req.Body.Disable,
                    Salt = user.Salt,
                    HashedText = user.HashedText,
                }, ct).ConfigureAwait(false);
                await SendNoContentAsync(cancellation: ct).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                MakeException(e, [
                    (HumanUser.EmailIndex, LocalCulture.Link(HumanResourcesFlag.EmailIndex)),
                    (HumanUser.UserNoIndex, LocalCulture.Link(HumanResourcesFlag.UserNoIndex)),
                ]);
            }
        }, [
            TableLayout.LetSelect<HumanUser>(req.Body.Id),
        ]);
    }
    public required IValidator<PutAccountRegisterInput> Validator { get; init; }
}