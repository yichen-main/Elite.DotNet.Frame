namespace Eywa.Serve.Constructs.Foundations.Quarterlies;
public interface ITubeRepository
{
    WebApplicationBuilder Add(in Action<WebApplicationBuilder> builder);
}