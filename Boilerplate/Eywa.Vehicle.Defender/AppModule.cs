namespace Eywa.Vehicle.Defender;

[DependsOn(
    typeof(EywaDomainHumanResourcesModule),
    typeof(EywaDomainMaterialPlanningModule),
    typeof(EywaDomainProductionControlModule),
    typeof(EywaDomainFacilityManagementModule))]
public sealed class AppModule : NodeModule<AppModule>
{
    protected override void Load(ContainerBuilder builder)
    {
        Execute(builder);
    }
}