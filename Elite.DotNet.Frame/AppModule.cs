using Autofac;
using Elite.Core.Architects.Primaries.Attributes;
using Elite.Core.Architects.Primaries.Composers;
using Elite.Domain.TestA;

namespace Elite.DotNet.Frame;

[DependsOn(typeof(EliteDomainTestAModule))]
public class AppModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Initial(this, builder);
    }
}