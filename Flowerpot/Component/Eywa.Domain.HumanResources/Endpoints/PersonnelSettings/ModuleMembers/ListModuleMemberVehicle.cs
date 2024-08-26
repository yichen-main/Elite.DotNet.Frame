namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class ListModuleMemberVehicle : NodeEnlarge<ListModuleMemberVehicle, ListModuleMemberInput>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(ListModuleMemberInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            List<GetModuleMemberVehicle.Output> outputs = [];
            await VerifyHumanMemberAsync().ConfigureAwait(false);
            var reader = await readerAsync.ConfigureAwait(false);
            var users = await reader.ReadAsync<HumanUser>().ConfigureAwait(false);
            var modules = await reader.ReadAsync<HumanUserModule>().ConfigureAwait(false);
            foreach (var module in modules)
            {
                var user = users.First(x => x.Id.IsMatch(module.UserId));
                outputs.Add(new()
                {
                    Id = module.Id,
                    Disable = module.Disable,
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
            Pagination(new PageContents<GetModuleMemberVehicle.Output>(results, req.PageCount, req.PageSize),
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