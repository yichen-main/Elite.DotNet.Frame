namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class AddModuleMemberVehicle() : NodeEnlarge<AddModuleMemberVehicle, AddModuleMemberInput>(RolePolicy.Owner)
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(AddModuleMemberInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            try
            {
                var userName = GetUserName();
                var id = TableLayout.GetSnowflakeId();
                await VerifyHumanMemberAsync().ConfigureAwait(false);
                await Validator.LeachAsync(req).ConfigureAwait(false);
                var reader = await readerAsync.ConfigureAwait(false);
                var loginUser = await reader.ReadFirstAsync<HumanUser>().ConfigureAwait(false);
                var user = await reader.ReadFirstOrDefaultAsync<HumanUser>().ConfigureAwait(false)
                ?? throw new Exception(LocalCulture.Link(HumanResourcesFlag.UserIdDoesNotExist));
                var modules = await reader.ReadAsync<HumanUserModule>().ConfigureAwait(false);
                var loginUserModule = modules.First(x => x.UserId.IsMatch(loginUser.Id));
                if (modules.Any(x => x.UserId.IsMatch(user.Id) && x.RolePolicy == loginUserModule.RolePolicy))
                    throw new Exception(LocalCulture.Link(HumanResourcesFlag.ModuleUserAlreadyExists));
                await NpgsqlHelper.InsertAsync(connection, new HumanUserModule
                {
                    Id = id,
                    UserId = user.Id,
                    RolePolicy = req.Body.AccessType,
                    DomainType = loginUserModule.DomainType,
                    Disable = req.Body.Disable,
                    Modifier = userName,
                    Creator = userName,
                    ModifyTime = DateTime.UtcNow,
                }, ct).ConfigureAwait(false);
                await SendCreatedAtAsync<GetModuleMemberVehicle>(new
                {
                    id,
                }, default, cancellation: ct).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                MakeException(e);
            }
        }, [
            TableLayout.LetSelect<HumanUser>(nameof(HumanUser.Id), GetUserId()),
            TableLayout.LetSelect<HumanUser>(nameof(HumanUser.Id), req.Body.UserId),
            TableLayout.LetSelect<HumanUserModule>(),
        ]);
    }
    public required IValidator<AddModuleMemberInput> Validator { get; init; }
}