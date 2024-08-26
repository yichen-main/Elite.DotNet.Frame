namespace Eywa.Serve.Modularity.Commons.Attacheds.Substances;
public class OneselfMutator
{
    public ValueTask<string> SetMongoDBAsync(string body, [Service] IMediator mediator)
    {
        return new();
    }
}