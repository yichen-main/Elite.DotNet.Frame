using NetLocalizer;
using Volo.Abp;

using (var application = AbpApplicationFactory.Create<AppModule>(options =>
{
    options.UseAutofac(); // 使用 Autofac 作為 DI 容器
}))
{
    await application.InitializeAsync();

    //https://blog.csdn.net/weixin_39842602/article/details/127571634

    // 使用應用程序
    //var myApp = application.ServiceProvider.GetRequiredService<MyApplication>();
    //myApp.Run();
}

//var supportedCultures = new[]
//       {
//            new CultureInfo("en-US"),
//            new CultureInfo("fr-FR"),
//            new CultureInfo("es-ES"),
//            // 添加更多支持的語言
//        };
//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
