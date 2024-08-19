using Microsoft.Extensions.Localization;

namespace Elite.DotNet.Frame.Foundations;
public class SomeService
{
    private readonly IStringLocalizerFactory _localizerFactory;

    public SomeService(IStringLocalizerFactory localizerFactory)
    {
        _localizerFactory = localizerFactory;
    }

    public void DoSomething()
    {
        using (CultureHelper.Use("zh-CN"))
        {
            var localizer = _localizerFactory.Create(typeof(SomeService));
            var message = localizer["Welcome"];
            Console.WriteLine(message);
        }
    }
}