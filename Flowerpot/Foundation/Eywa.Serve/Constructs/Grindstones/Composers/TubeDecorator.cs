namespace Eywa.Serve.Constructs.Grindstones.Composers;
internal abstract class TubeDecorator(ITubeRepository tubeRepository) : ITubeRepository
{
    public virtual WebApplicationBuilder Add(in Action<WebApplicationBuilder> builder) => tubeRepository.Add(builder);
}