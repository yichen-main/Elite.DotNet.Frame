namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleSupervisors;
internal sealed class PutModuleSupervisorVehicle() : NodeEnlarge<PutModuleSupervisorVehicle, PutModuleSupervisorInput>(RolePolicy.Editor)
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(PutModuleSupervisorInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            try
            {
                await VerifyHumanMemberAsync().ConfigureAwait(false);
                await Validator.LeachAsync(req).ConfigureAwait(false);
                var reader = await readerAsync.ConfigureAwait(false);
                var modules = await reader.ReadAsync<HumanUserModule>().ConfigureAwait(false);
                var user = await reader.ReadSingleAsync<HumanUser>().ConfigureAwait(false);
                if (modules.Any(x => x.DomainType == req.Body.DomainType))
                    throw new Exception(LocalCulture.Link(HumanResourcesFlag.DomainTypeExisted));
                var module = modules.First(x => x.Id.IsMatch(req.Body.Id));
                if (module is not null) await NpgsqlHelper.UpdateAsync(module.Id, connection, new HumanUserModule
                {
                    Id = req.Body.Id,
                    UserId = user.Id,
                    DomainType = req.Body.DomainType,
                    RolePolicy = RolePolicy.Owner,
                    Disable = req.Body.Disable,
                    Modifier = GetUserName(),
                    Creator = module.Creator,
                    ModifyTime = DateTime.UtcNow,
                }, ct).ConfigureAwait(false);
                await SendNoContentAsync(cancellation: ct).ConfigureAwait(false);
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
    public required IValidator<PutModuleSupervisorInput> Validator { get; init; }
}