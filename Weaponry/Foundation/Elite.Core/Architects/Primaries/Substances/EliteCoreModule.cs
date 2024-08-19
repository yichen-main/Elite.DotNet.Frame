namespace Elite.Core.Architects.Primaries.Substances;
public sealed class EliteCoreModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Initial(this, builder);
    }
}