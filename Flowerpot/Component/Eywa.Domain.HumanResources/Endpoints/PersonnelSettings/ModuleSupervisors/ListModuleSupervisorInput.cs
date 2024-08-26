namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleSupervisors;
internal sealed class ListModuleSupervisorInput : NodeQueryer
{
    public string? UserId { get; init; }
    public DomainType? DomainType { get; init; }
}