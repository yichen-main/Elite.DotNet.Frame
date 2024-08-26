namespace Eywa.Serve.Constructs.Grindstones.Assemblies;
internal sealed class FiberRepository(WebApplicationBuilder builder, ITubeRepository tubeRepository) : TubeDecorator(tubeRepository)
{
    public override WebApplicationBuilder Add(in Action<WebApplicationBuilder>? builder)
    {
        if (builder is not null) builder(Builder);
        return Builder;
    }
    WebApplicationBuilder Builder { get; init; } = builder;
}