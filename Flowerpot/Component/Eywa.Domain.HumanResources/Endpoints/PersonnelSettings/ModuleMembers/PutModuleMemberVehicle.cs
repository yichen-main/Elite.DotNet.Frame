namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class PutModuleMemberVehicle() : NodeEnlarge<PutModuleMemberVehicle, PutModuleMemberInput>(RolePolicy.Editor)
{
    public override void Configure()
    {
        AllowFormData(urlEncoded: true);
        RequiredConfiguration();
    }
    public override Task HandleAsync(PutModuleMemberInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            await VerifyHumanMemberAsync().ConfigureAwait(false);
            await Validator.LeachAsync(req).ConfigureAwait(false);
            var reader = await readerAsync.ConfigureAwait(false);
            var loginUser = await reader.ReadFirstAsync<HumanUser>().ConfigureAwait(false);
            var user = await reader.ReadFirstOrDefaultAsync<HumanUser>().ConfigureAwait(false)
            ?? throw new Exception(LocalCulture.Link(HumanResourcesFlag.UserIdDoesNotExist));
            var modules = await reader.ReadAsync<HumanUserModule>().ConfigureAwait(false);
            var loginUserModule = modules.First(x => x.UserId.IsMatch(loginUser.Id));
            if (modules.Any(x => x.UserId.IsMatch(req.Body.UserId) && x.RolePolicy == loginUserModule.RolePolicy))
                throw new Exception(LocalCulture.Link(HumanResourcesFlag.ModuleUserAlreadyExists));
            var module = modules.First(x => x.Id.IsMatch(req.Body.Id));
            if (module is not null) await NpgsqlHelper.UpdateAsync(module.Id, connection, new HumanUserModule
            {
                UserId = req.Body.UserId,
                RolePolicy = module.RolePolicy,
                DomainType = module.DomainType,
                Disable = req.Body.Disable,
                Modifier = GetUserName(),
                Creator = module.Creator,
                ModifyTime = DateTime.UtcNow,
            }, ct).ConfigureAwait(false);
            await SendNoContentAsync(cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(nameof(HumanUser.Id), GetUserId()),
            TableLayout.LetSelect<HumanUser>(nameof(HumanUser.Id), req.Body.UserId),
            TableLayout.LetSelect<HumanUserModule>(),
        ]);
    }
    public required IValidator<PutModuleMemberInput> Validator { get; init; }
}