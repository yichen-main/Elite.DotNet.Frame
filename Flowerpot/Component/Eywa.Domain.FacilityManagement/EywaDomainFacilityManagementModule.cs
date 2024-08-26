namespace Eywa.Domain.FacilityManagement;

[DependsOn(typeof(EywaServeModularityModule))]
public sealed class EywaDomainFacilityManagementModule : NodeModule<EywaDomainFacilityManagementModule>
{
    protected override void Load(ContainerBuilder builder)
    {
        Execute(builder);
    }
}