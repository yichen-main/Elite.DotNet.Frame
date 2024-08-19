namespace Elite.Core.Architects.Primaries.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ModularAttribute(string name = nameof(AsyncCallback)) : Attribute
{
    public string Name { get; init; } = name;
}