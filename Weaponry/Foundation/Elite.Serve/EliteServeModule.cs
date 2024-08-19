using Autofac;
using Elite.Core.Architects.Primaries.Composers;

namespace Elite.Serve;
public sealed class EliteServeModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Initial(this, builder);
    }
}