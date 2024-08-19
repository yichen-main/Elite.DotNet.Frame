using Elite.Core.Architects.Primaries.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Elite.Vehicle.Platform.NewFolder;
public interface IAppService
{
    void DoWork();
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class AppService : IAppService
{
    public void DoWork()
    {
        Console.WriteLine("Doing work... [AppService]");
    }
}