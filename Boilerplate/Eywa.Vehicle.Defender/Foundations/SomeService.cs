﻿namespace Eywa.Vehicle.Defender.Foundations;
public class SomeService(IStringLocalizerFactory localizerFactory)
{
    public void DoSomething()
    {
        using (CultureHelper.Use("zh-CN"))
        {
            var localizer = localizerFactory.Create(typeof(SomeService));
            var message = localizer["Welcome"];
            Console.WriteLine(message);
        }
    }
}