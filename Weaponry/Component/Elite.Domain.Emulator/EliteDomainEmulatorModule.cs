using Autofac;
using Elite.Core.Architects.Primaries.Attributes;
using Elite.Core.Architects.Primaries.Composers;
using Elite.Core.Architects.Primaries.Substances;

namespace Elite.Domain.Emulator;

[DependsOn(typeof(EliteCoreModule))]
public class EliteDomainEmulatorModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Initial(this, builder);
    }
}