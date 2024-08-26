namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleSupervisors;
internal sealed class AddModuleSupervisorVehicle() : NodeEnlarge<AddModuleSupervisorVehicle, AddModuleSupervisorInput>(RolePolicy.Editor)
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(AddModuleSupervisorInput req, CancellationToken ct)
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
                var modules = await reader.ReadAsync<HumanUserModule>().ConfigureAwait(false);
                var user = await reader.ReadSingleAsync<HumanUser>().ConfigureAwait(false);
                if (modules.Any(x => x.DomainType == req.Body.DomainType))
                    throw new Exception(LocalCulture.Link(HumanResourcesFlag.DomainTypeExisted));
                await NpgsqlHelper.InsertAsync(connection, new HumanUserModule
                {
                    Id = id,
                    UserId = user.Id,
                    DomainType = req.Body.DomainType,
                    RolePolicy = RolePolicy.Owner,
                    Disable = req.Body.Disable,
                    Modifier = userName,
                    Creator = userName,
                    ModifyTime = DateTime.UtcNow,
                }, ct).ConfigureAwait(false);
                await SendCreatedAtAsync<GetModuleSupervisorVehicle>(new
                {
                    id,
                }, default, cancellation: ct).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                MakeException(e);
            }
        }, [
            TableLayout.LetSelect<HumanUserModule>(),
            TableLayout.LetSelect<HumanUser>(req.Body.UserId),
        ]);
    }
    public required IValidator<AddModuleSupervisorInput> Validator { get; init; }
}