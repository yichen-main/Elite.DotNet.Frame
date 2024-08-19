namespace Elite.Core.Architects.Primaries.Substances;
internal sealed class ModuleLoader(ContainerBuilder builder)
{
    public void LoadModules(in Assembly? assembly)
    {
        if (assembly is not null)
        {
            foreach (var type in assembly.GetTypes().Where(x =>
            typeof(BaseModule).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)) LoadModule(type);
        }
    }
    void LoadModule(in Type moduleType)
    {
        if (LoadedModules.Contains(moduleType)) return;
        var attribute = moduleType.GetCustomAttribute<DependsOnAttribute>();
        if (attribute is not null)
        {
            foreach (var dependency in attribute.Dependencies) LoadModule(dependency);
        }
        var module = Activator.CreateInstance(moduleType);
        if (module is not null) builder.RegisterModule((BaseModule)module);
        LoadedModules.Add(moduleType);
    }
    List<Type> LoadedModules { get; set; } = [];
}