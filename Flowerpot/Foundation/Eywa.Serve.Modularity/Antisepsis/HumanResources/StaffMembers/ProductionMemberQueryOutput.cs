namespace Eywa.Serve.Modularity.Antisepsis.HumanResources.StaffMembers;
public readonly struct ProductionMemberQueryOutput
{
    public required string UserId { get; init; }
    public required string UserNo { get; init; }
    public required string UserName { get; init; }
    public required RolePolicy RolePolicy { get; init; }
}