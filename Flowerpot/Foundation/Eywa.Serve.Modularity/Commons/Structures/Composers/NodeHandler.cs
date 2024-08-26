namespace Eywa.Serve.Modularity.Commons.Structures.Composers;
public abstract class NodeHandler<T1, T2> : IRequestHandler<T1, T2> where T1 : IRequest<T2> where T2 : notnull
{
    protected abstract Task<T2> HandleAsync(T1 import, NpgsqlConnection connection, CancellationToken ct);
    public Task<T2> Handle(T1 import, CancellationToken ct) => NpgsqlHelper.ConnectAsync(x => HandleAsync(import, x, ct));
    public required INpgsqlHelper NpgsqlHelper { get; init; }
}