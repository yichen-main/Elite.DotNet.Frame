namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleSupervisors;
internal sealed class ListModuleSupervisorVehicle : NodeEnlarge<ListModuleSupervisorVehicle, ListModuleSupervisorInput>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(ListModuleSupervisorInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            List<GetModuleSupervisorVehicle.Output> outputs = [];
            await VerifyHumanMemberAsync().ConfigureAwait(false);
            var reader = await readerAsync.ConfigureAwait(false);
            var users = await reader.ReadAsync<HumanUser>().ConfigureAwait(false);
            var modules = await reader.ReadAsync<HumanUserModule>().ConfigureAwait(false);
            foreach (var module in modules.Where(module => module.RolePolicy is RolePolicy.Owner))
            {
                var user = users.First(x => x.Id.IsMatch(module.UserId));
                outputs.Add(new()
                {
                    Id = module.Id,
                    Disable = user.Disable,
                    DomainType = module.DomainType,
                    DomainName = LocalCulture.Link(module.DomainType),
                    AccessName = LocalCulture.Link(module.RolePolicy),
                    UserId = user.Id,
                    Email = user.Email,
                    UserNo = user.UserNo,
                    UserName = user.UserName,
                    Creator = module.Creator,
                    CreateTime = TableLayout.LocalTime(module.CreateTime, req.TimeZone, req.TimeFormat),
                    Modifier = module.Modifier,
                    ModifyTime = TableLayout.LocalTime(module.ModifyTime, req.TimeZone, req.TimeFormat),
                });
            }
            var results = req.UserId == default ? outputs : outputs.Where(x => x.UserId.IsMatch(req.UserId));
            results = req.DomainType == default ? outputs : outputs.Where(x => x.DomainType == req.DomainType);
            Pagination(new PageContents<GetModuleSupervisorVehicle.Output>(results, req.PageCount, req.PageSize),
            [
                (nameof(req.Search), req.Search),
                (nameof(req.UserId), req.UserId),
                (nameof(req.DomainType), req.DomainType.ToString()),
            ]);
            await SendAsync(results, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(),
            TableLayout.LetSelect<HumanUserModule>(),
        ]);
    }
}