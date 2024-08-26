namespace Eywa.Serve.Modularity.Commons.Structures.Composers;
public abstract class NodeModule<TModule> : BaseModule where TModule : BaseModule
{
    protected void Execute(in ContainerBuilder builder)
    {
        var type = Initialize(this, builder);
        DomainType = GetDomainType(type.Name);
        builder.Populate(new ServiceCollection().AddServices<TModule>());
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    DomainType GetDomainType(string input)
    {
        foreach (var name in Enum.GetNames(typeof(DomainType)).Where(x => input.IsFuzzy(x)))
        {
            return ((DomainType)Enum.Parse(typeof(DomainType), name, ignoreCase: true)).Archive();
        }
        return DomainType.Unrecognizable;
    }
    protected DomainType DomainType { get; private set; }
}