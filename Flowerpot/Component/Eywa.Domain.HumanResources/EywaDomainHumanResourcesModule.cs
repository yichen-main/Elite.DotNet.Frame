namespace Eywa.Domain.HumanResources;

[DependsOn(typeof(EywaServeModularityModule))]
public sealed class EywaDomainHumanResourcesModule : NodeModule<EywaDomainHumanResourcesModule>
{
    protected override void Load(ContainerBuilder builder)
    {
        Execute(builder);
    }
}