namespace Eywa.Serve.Constructs.Grindstones.Assemblies;
internal sealed class TubeRepository(WebApplicationBuilder webApplicationBuilder) : ITubeRepository
{
    public WebApplicationBuilder Add(in Action<WebApplicationBuilder> builder)
    {
        builder(webApplicationBuilder);
        return webApplicationBuilder;
    }
}