namespace Eywa.Vehicle.Defender.Foundations;
public class LoggingProxy<T> : DispatchProxy
{
    T _target;
    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        bool shouldLog = targetMethod.GetCustomAttributes(typeof(LogAttribute), true).Any();
        if (shouldLog)
        {
            Console.WriteLine($"[LOG] Before executing {targetMethod.Name}");
        }

        var result = targetMethod.Invoke(_target, args);
        if (shouldLog)
        {
            Console.WriteLine($"[LOG] After executing {targetMethod.Name}");
        }
        return result;
    }
    public static T Create(T target)
    {
        var proxy = Create<T, LoggingProxy<T>>() as LoggingProxy<T>;
        proxy._target = target;
        return (T)(object)proxy;
    }
}