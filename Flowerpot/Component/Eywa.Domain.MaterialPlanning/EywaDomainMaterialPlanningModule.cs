namespace Eywa.Domain.MaterialPlanning;

[DependsOn(typeof(EywaServeModularityModule))]
public class EywaDomainMaterialPlanningModule : NodeModule<EywaDomainMaterialPlanningModule>
{
    protected override void Load(ContainerBuilder builder)
    {
        Execute(builder);
    }
}