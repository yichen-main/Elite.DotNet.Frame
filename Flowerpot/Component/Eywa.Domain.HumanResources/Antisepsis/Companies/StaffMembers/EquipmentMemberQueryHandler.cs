namespace Eywa.Domain.HumanResources.Antisepsis.Companies.StaffMembers;
internal sealed class EquipmentMemberQueryHandler : NodeHandler<EquipmentMemberQueryImport, IEnumerable<EquipmentMemberQueryOutput>>
{
    protected override async Task<IEnumerable<EquipmentMemberQueryOutput>> HandleAsync(EquipmentMemberQueryImport import, NpgsqlConnection connection, CancellationToken ct)
    {
        var readers = await TableLayout.MultipleQueriesAsync(connection, [
            TableLayout.LetSelect<HumanUser>(),
            TableLayout.LetSelect<HumanUserModule>(),
        ]).ConfigureAwait(false);
        List<EquipmentMemberQueryOutput> outputs = [];
        var users = await readers.ReadAsync<HumanUser>().ConfigureAwait(false);
        var modules = await readers.ReadAsync<HumanUserModule>().ConfigureAwait(false);
        foreach (var module in modules.Where(x => x is { Disable: false, DomainType: DomainType.FacilityManagement }))
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