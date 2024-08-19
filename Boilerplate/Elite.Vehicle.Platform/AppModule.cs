using Autofac;
using Elite.Core.Architects.Primaries.Attributes;
using Elite.Core.Architects.Primaries.Composers;
using Elite.Domain.Emulator;

namespace Elite.Vehicle.Platform;

[DependsOn(typeof(EliteDomainEmulatorModule))]
public class AppModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Initial(this, builder);
    }
}