namespace Elite.Core.Architects.Primaries.Composers;
public abstract class BaseModule : Autofac.Module
{
    protected void Initial<T>(in T content, in ContainerBuilder builder) where T : notnull
    {
        var types = content.GetType().GetAssemblyTypes();
        for (var i = default(int); i < types.Length; i++)
        {
            var type = types[i];
            switch (type)
            {
                case var x when x.GetCustomAttributes<DependentAttribute>().Any():
                    {
                        var registration = builder.RegisterType(type);
                        var attribute = type.GetCustomAttribute<DependentAttribute>();
                        if (attribute is not null)
                        {
                            switch (attribute.Lifetime)
                            {
                                case ServiceLifetime.Singleton:
                                    registration.SingleInstance();
                                    break;

                                case ServiceLifetime.Transient:
                                    registration.InstancePerDependency();
                                    break;

                                case ServiceLifetime.Scoped:
                                    registration.InstancePerLifetimeScope();
                                    break;
                            }
                        }
                        registration.AsImplementedInterfaces();
                    }
                    break;

                case var x when x.IsSubclassOf(typeof(HostedService)):
                    builder.RegisterType(type).As<IHostedService>().InstancePerDependency();
                    break;
            }
        }
    }
}