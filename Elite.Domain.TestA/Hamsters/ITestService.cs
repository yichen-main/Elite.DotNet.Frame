using Elite.Core.Architects.Primaries.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Elite.Domain.TestA.Hamsters;

public interface ITestService
{
    void DoWork();
}

[Dependent(ServiceLifetime.Singleton)]
public sealed class TestService : ITestService
{
    public void DoWork()
    {
        Console.WriteLine("Doing work...");
    }
}