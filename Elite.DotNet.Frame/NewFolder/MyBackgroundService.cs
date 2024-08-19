using Elite.Domain.TestA.Hamsters;
using Elite.DotNet.Frame.Controllers;
using Elite.DotNet.Frame.Foundations;
using Microsoft.Extensions.Localization;

namespace Elite.DotNet.Frame.NewFolder;
public class MyBackgroundService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var kk = AppService;
        var ll = TestService;

        //en-US zh-CN zh-TW
        using (CultureHelper.Use("zh-TW"))
        {
            var localizedString = Localizer["UserIdIsRequired"];

            var moduleName = Localizer["DeviceIdIsRequired"].ToString();
        }
        AppService.DoWork();
        TestService.DoWork();
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public required IAppService AppService { get; init; }
    public required ITestService TestService { get; init; }
    public required IStringLocalizer<WeatherForecastController> Localizer { get; init; }
}