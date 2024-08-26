namespace Eywa.Domain.ProductionControl;

[DependsOn(typeof(EywaServeModularityModule))]
public sealed class EywaDomainProductionControlModule : NodeModule<EywaDomainProductionControlModule>
{
    protected override void Load(ContainerBuilder builder)
    {
        Execute(builder);
    }
}