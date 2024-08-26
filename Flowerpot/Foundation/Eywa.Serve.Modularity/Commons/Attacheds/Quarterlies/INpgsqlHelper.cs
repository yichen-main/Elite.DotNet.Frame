namespace Eywa.Serve.Modularity.Commons.Attacheds.Quarterlies;
public interface INpgsqlHelper
{
    Task CreateAsync(in Type type);
    Task ConnectAsync(Func<NpgsqlConnection, Task> npgsql);
    Task<T> ConnectAsync<T>(Func<NpgsqlConnection, Task<T>> npgsql) where T : notnull;
    Task TransactionAsync(Func<NpgsqlConnection, NpgsqlTransaction, Task> options, CancellationToken ct);
    Task<T> TransactionAsync<T>(Func<NpgsqlConnection, NpgsqlTransaction, Task<T>> options, CancellationToken ct) where T : NpgsqlBase;
    Task<T?> SelectAsync<T>(in NpgsqlConnection connection, in string id, in CancellationToken ct) where T : NpgsqlBase;
    Task<IEnumerable<T>> SelectAsync<T>(in NpgsqlConnection connection, in CancellationToken ct) where T : NpgsqlBase;
    Task InsertAsync<T>(in NpgsqlConnection connection, in T entity, in CancellationToken ct) where T : NpgsqlBase;
    Task DeleteAsync<T>(in string id, in NpgsqlConnection connection) where T : NpgsqlBase;
    Task UpdateAsync<T>(in string id, in NpgsqlConnection connection, in T entity, in CancellationToken ct, in string[]? fields = default) where T : NpgsqlBase;
    void SetConfiguration(in string? connectionString);
    string? ConnectionString { get; }
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class NpgsqlHelper : INpgsqlHelper
{
    public Task CreateAsync(in Type type)
    {
        List<string> columns = [];
        List<string> indexes = [];
        var properties = type.GetProperties();
        for (int i = default; i < properties.Length; i++)
        {
            var property = properties[i];
            var columnName = property.Name.ToSnakeCase();
            var rowInfo = property.GetCustomAttribute<RowInfoAttribute>();
            if (rowInfo is { UniqueIndex: true }) indexes.Add(TableLayout.LetUniqueIndex(type.Name.ToSnakeCase(), columnName));
            switch (property.Name)
            {
                case nameof(NpgsqlBase.Id):
                    columns.Add($"{columnName} CHARACTER(18) PRIMARY KEY");
                    break;

                default:
                    if (rowInfo is { ForeignKey: true })
                    {
                        columns.Add($"{columnName} CHARACTER(18) NOT NULL");
                    }
                    else columns.Add($"{columnName} {property.PropertyType switch
                    {
                        var x when x.IsEnum => "SMALLINT",
                        var x when x.Equals(typeof(Guid)) => "UUID",
                        var x when x.Equals(typeof(bool)) => "BOOLEAN",
                        var x when x.Equals(typeof(short)) => "SMALLINT",
                        var x when x.Equals(typeof(int)) => "INTEGER",
                        var x when x.Equals(typeof(long)) => "BIGINT",
                        var x when x.Equals(typeof(float)) => "REAL",
                        var x when x.Equals(typeof(double)) => "DOUBLE PRECISION",
                        var x when x.Equals(typeof(decimal)) => "MONEY",
                        var x when x.Equals(typeof(DateTime)) => "TIMESTAMP WITHOUT TIME ZONE",
                        _ => "VARCHAR"
                    }} NOT NULL");
                    break;
            }
        }
        var sql = TableLayout.LetCreate(type, columns);
        return ConnectAsync(async x =>
        {
            await x.ExecuteAsync(sql).ConfigureAwait(false);
            foreach (var index in indexes) await x.ExecuteAsync(index).ConfigureAwait(false);
        });
    }
    public async Task ConnectAsync(Func<NpgsqlConnection, Task> npgsql)
    {
        NpgsqlConnection connection = new(ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            await npgsql(connection).ConfigureAwait(false);
        }
    }
    public async Task<T> ConnectAsync<T>(Func<NpgsqlConnection, Task<T>> npgsql) where T : notnull
    {
        NpgsqlConnection connection = new(ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            return await npgsql(connection).ConfigureAwait(false);
        }
    }
    public Task TransactionAsync(Func<NpgsqlConnection, NpgsqlTransaction, Task> options, CancellationToken ct)
    {
        return ConnectAsync(async npgsql =>
        {
            await npgsql.OpenAsync(ct).ConfigureAwait(false);
            var transaction = await npgsql.BeginTransactionAsync(ct).ConfigureAwait(false);
            try
            {
                await options(npgsql, transaction).ConfigureAwait(false);
                await transaction.CommitAsync(ct).ConfigureAwait(false);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct).ConfigureAwait(false);
                throw;
            }
            finally
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
                await npgsql.CloseAsync().ConfigureAwait(false);
            }
        });
    }
    public Task<T> TransactionAsync<T>(Func<NpgsqlConnection, NpgsqlTransaction, Task<T>> options, CancellationToken ct) where T : NpgsqlBase
    {
        return ConnectAsync(async npgsql =>
        {
            await npgsql.OpenAsync(ct).ConfigureAwait(false);
            var transaction = await npgsql.BeginTransactionAsync(ct).ConfigureAwait(false);
            try
            {
                var result = await options(npgsql, transaction).ConfigureAwait(false);
                await transaction.CommitAsync(ct).ConfigureAwait(false);
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct).ConfigureAwait(false);
                throw;
            }
            finally
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
                await npgsql.CloseAsync().ConfigureAwait(false);
            }
        });
    }
    public Task<T?> SelectAsync<T>(in NpgsqlConnection connection, in string id, in CancellationToken ct) where T : NpgsqlBase
    {
        return new NpgsqlRepository<T>(connection).GetAsync(id, ct);
    }
    public Task<IEnumerable<T>> SelectAsync<T>(in NpgsqlConnection connection, in CancellationToken ct) where T : NpgsqlBase
    {
        return new NpgsqlRepository<T>(connection).GetAsync(ct);
    }
    public Task InsertAsync<T>(in NpgsqlConnection connection, in T entity, in CancellationToken ct) where T : NpgsqlBase
    {
        return new NpgsqlRepository<T>(connection).AddAsync(entity, ct);
    }
    public Task DeleteAsync<T>(in string id, in NpgsqlConnection connection) where T : NpgsqlBase
    {
        return new NpgsqlRepository<T>(connection).ClearAsync(id);
    }
    public Task UpdateAsync<T>(in string id, in NpgsqlConnection connection, in T entity, in CancellationToken ct, in string[]? fields = default) where T : NpgsqlBase
    {
        return new NpgsqlRepository<T>(connection).PutAsync(id, entity, ct, fields);
    }
    public void SetConfiguration(in string? connectionString) => ConnectionString = connectionString;
    public string? ConnectionString { get; private set; }
}