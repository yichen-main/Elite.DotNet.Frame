namespace Eywa.Serve.Constructs.Foundations.Quarterlies;
public interface IEditionRepository
{
    WebApplicationBuilder Initialize(Assembly? assembly);
    WebApplicationBuilder Add(in WebApplicationBuilder builder, in Action<KestrelServerOptions> options);
    WebApplication Add(in WebApplicationBuilder builder, Action<WebApplication, IEndpointRouteBuilder> options);
}