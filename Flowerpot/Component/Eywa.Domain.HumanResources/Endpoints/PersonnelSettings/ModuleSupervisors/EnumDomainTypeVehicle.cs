namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleSupervisors;
internal sealed class EnumDomainTypeVehicle : NodeEnlarge<EnumDomainTypeVehicle, NodeUnique>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(NodeUnique req, CancellationToken ct)
    {
        VerifyRootMember();
        List<EnumCistern> results = [];
        foreach (var module in BeforeExpand.GetModules()) results.Add(new()
        {
            TypeNo = module.GetHashCode(),
            TypeName = LocalCulture.Link(module),
        });
        return SendAsync(results, cancellation: ct);
    }
}