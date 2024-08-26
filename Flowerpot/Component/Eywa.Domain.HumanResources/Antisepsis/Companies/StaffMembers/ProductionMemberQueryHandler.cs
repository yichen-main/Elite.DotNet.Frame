namespace Eywa.Domain.HumanResources.Antisepsis.Companies.StaffMembers;
internal sealed class ProductionMemberQueryHandler : NodeHandler<ProductionMemberQueryImport, IEnumerable<ProductionMemberQueryOutput>>
{
    protected override async Task<IEnumerable<ProductionMemberQueryOutput>> HandleAsync(ProductionMemberQueryImport import, NpgsqlConnection connection, CancellationToken ct)
    {
        var readers = await TableLayout.MultipleQueriesAsync(connection, [
            TableLayout.LetSelect<HumanUser>(),
            TableLayout.LetSelect<HumanUserModule>(),
        ]).ConfigureAwait(false);
        List<ProductionMemberQueryOutput> outputs = [];
        var users = await readers.ReadAsync<HumanUser>().ConfigureAwait(false);
        var modules = await readers.ReadAsync<HumanUserModule>().ConfigureAwait(false);
        foreach (var module in modules.Where(x => x is { Disable: false, DomainType: DomainType.ProductionControl }))
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