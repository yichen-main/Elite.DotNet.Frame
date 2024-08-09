using System.Globalization;

namespace NetLocalizer.Foundations;
public static class CultureHelper
{
    public static IDisposable Use(string culture)
    {
        var currentCulture = CultureInfo.CurrentCulture;
        var currentUICulture = CultureInfo.CurrentUICulture;

        var newCulture = new CultureInfo(culture);
        var newUICulture = new CultureInfo(culture);

        CultureInfo.CurrentCulture = newCulture;
        CultureInfo.CurrentUICulture = newUICulture;

        return new DisposableCulture(currentCulture, currentUICulture);
    }

    private class DisposableCulture(CultureInfo originalCulture, CultureInfo originalUICulture) : IDisposable
    {
        public void Dispose()
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }
}

//public class HomeController : Controller
//{
//    private readonly IStringLocalizer<HomeController> _localizer;

//    public HomeController(IStringLocalizer<HomeController> localizer)
//    {
//        _localizer = localizer;
//    }

//    public IActionResult Index()
//    {
//        using (CultureHelper.Use("en-US"))
//        {
//            ViewData["Title"] = _localizer["HomeTitle"];
//        }

//        return View();
//    }
//}