namespace Eywa.Domain.HumanResources.Antisepsis.Companies.StaffMembers;
internal sealed class HumanMemberQueryHandler : NodeHandler<HumanMemberQueryImport, IEnumerable<HumanMemberQueryOutput>>
{
    protected override async Task<IEnumerable<HumanMemberQueryOutput>> HandleAsync(HumanMemberQueryImport import, NpgsqlConnection connection, CancellationToken ct)
    {
        var readers = await TableLayout.MultipleQueriesAsync(connection, [
            TableLayout.LetSelect<HumanUser>(),
            TableLayout.LetSelect<HumanUserModule>(),
        ]).ConfigureAwait(false);
        List<HumanMemberQueryOutput> outputs = [];
        var users = await readers.ReadAsync<HumanUser>().ConfigureAwait(false);
        var modules = await readers.ReadAsync<HumanUserModule>().ConfigureAwait(false);
        foreach (var module in modules.Where(x => x is { Disable: false, DomainType: DomainType.HumanResources }))
        {
            var user = users.First(x => x.Id.IsMatch(module.UserId));
            outputs.Add(new()
            {
                UserId = module.UserId,
                UserNo = user.UserNo,
                UserName = user.UserName,
                RolePolicy = module.RolePolicy,
            });
        }
        return outputs.AsEnumerable();
    }
}