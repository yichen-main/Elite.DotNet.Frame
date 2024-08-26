namespace Eywa.Vehicle.Defender.Foundations;
public interface IMyService
{
    [Log] void DoSomething(string param);
    void DoSomethingElse(int number);
}

[Dependent(ServiceLifetime.Singleton)]
file class MyService : IMyService
{
    public void DoSomething(string param)
    {
        Console.WriteLine($"Doing something with {param}");
    }
    public void DoSomethingElse(int number)
    {
        Console.WriteLine($"Doing something else with {number}");
    }
}