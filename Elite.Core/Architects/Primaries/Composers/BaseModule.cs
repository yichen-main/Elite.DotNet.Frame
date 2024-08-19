namespace Elite.Core.Architects.Primaries.Composers;
public abstract class BaseModule : Autofac.Module
{
    protected void Initial<T>(in T content, in ContainerBuilder builder) where T : notnull
    {
        foreach (var type in content.GetType().GetSpecificTypes<DependentAttribute>())
        {
            var registration = builder.RegisterType(type);
            switch (((DependentAttribute)type.GetCustomAttributes(typeof(DependentAttribute), false).First()).Lifetime)
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
            registration.AsImplementedInterfaces();
        }
    }
}