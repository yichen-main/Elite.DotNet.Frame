using Autofac;
using Elite.Core.Architects.Primaries.Attributes;
using Elite.Core.Architects.Primaries.Composers;
using Elite.Core.Architects.Primaries.Substances;

namespace Elite.Domain.TestA;

[DependsOn(typeof(EliteCoreModule))]
public class EliteDomainTestAModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Initial(this, builder);
    }
}