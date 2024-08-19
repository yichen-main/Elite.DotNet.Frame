using Elite.Core.Architects.Primaries.Composers;
using Elite.Domain.Emulator.Hamsters;
using Elite.Vehicle.Platform.Foundations;
using Microsoft.Extensions.Localization;

namespace Elite.Vehicle.Platform.NewFolder;
public class MyBackgroundService : HostedService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
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
    public required IAppService AppService { get; init; }
    public required ITestService TestService { get; init; }
    public required IStringLocalizer<MyBackgroundService> Localizer { get; init; }
}