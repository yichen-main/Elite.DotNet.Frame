namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class ListModuleMemberInput : NodeQueryer
{
    public string? UserId { get; init; }
    public DomainType? DomainType { get; init; }
}