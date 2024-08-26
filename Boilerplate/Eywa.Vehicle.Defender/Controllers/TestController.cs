namespace Eywa.Vehicle.Defender.Controllers;

[Route("ass")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var moduleName = LocalCulture.Link(HumanResourcesFlag.PositionNameIsRequired);
            return Ok(moduleName);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    public required ILocalCulture LocalCulture { get; init; }
}