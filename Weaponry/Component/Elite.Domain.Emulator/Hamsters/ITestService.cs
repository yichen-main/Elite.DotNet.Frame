using Elite.Core.Architects.Primaries.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Elite.Domain.Emulator.Hamsters;
public interface ITestService
{
    void DoWork();
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class TestService : ITestService
{
    public void DoWork()
    {
        Console.WriteLine("Doing work...");
    }
}