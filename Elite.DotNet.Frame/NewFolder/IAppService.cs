using Elite.Core.Architects.Primaries.Attributes;

namespace Elite.DotNet.Frame.NewFolder;

public interface IAppService
{
    void DoWork();
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class AppService : IAppService
{
    public void DoWork()
    {
        Console.WriteLine("Doing work...");
    }
}