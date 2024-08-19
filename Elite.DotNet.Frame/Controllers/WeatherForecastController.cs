using Elite.DotNet.Frame.NewFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Elite.DotNet.Frame.Controllers
{
    [ApiController]
    [Route("ass")]
    public class WeatherForecastController(IStringLocalizer<WeatherForecastController> localizer, IAppService MyService) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var kk = MyService;
            var localizedString = localizer["UserIdIsRequired"];

            var moduleName = localizer["DeviceIdIsRequired"].ToString();
            // ... 其他邏輯 ...
            return Ok(new { ModuleName = moduleName });
        }
        //public required IAppService MyService { get; init; }
    }
}