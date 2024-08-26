namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class EnumAccessTypeVehicle : NodeEnlarge<EnumAccessTypeVehicle, NodeUnique>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override async Task HandleAsync(NodeUnique req, CancellationToken ct)
    {
        List<EnumCistern> results = [];
        var verifyAsync = VerifyHumanMemberAsync();
        foreach (var type in Enum.GetValues<RolePolicy>())
        {
            if (type is RolePolicy.Editor || type is RolePolicy.Viewer) results.Add(new()
            {
                TypeNo = type.GetHashCode(),
                TypeName = LocalCulture.Link(type),
            });
        }
        await verifyAsync.ConfigureAwait(false);
        await SendAsync(results, cancellation: ct).ConfigureAwait(false);
    }
}