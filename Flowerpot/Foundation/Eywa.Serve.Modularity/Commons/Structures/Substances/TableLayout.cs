namespace Eywa.Serve.Modularity.Commons.Structures.Substances;
public readonly ref struct TableLayout
{
    public static string LetSelect(in Type type)
    {
        List<string> columns = [];
        foreach (var property in type.GetProperties()) columns.Add(property.Name.ToSnakeCase());
        return $"SELECT {Join(columns)} FROM {type.Name.ToSnakeCase()} {Sequencer}";
    }
    public static string LetSelect<T>() => LetSelect(typeof(T));
    public static string LetSelect(in Type type, in string? identifyName = null)
    {
        List<string> columns = [];
        foreach (var property in type.GetProperties()) columns.Add(property.Name.ToSnakeCase());
        return $"SELECT {Join(columns)} FROM {type.Name.ToSnakeCase()} {QueryCondition(identifyName)} {Sequencer}";
    }
    public static string LetSelect<T>(in string id) => LetSelect<T>(nameof(SQLiteBase.Id).ToSnakeCase(), id);
    public static string LetSelect<T>(in string columnName, in string? value)
    {
        var type = typeof(T);
        List<string> columns = [];
        foreach (var property in type.GetProperties()) columns.Add(property.Name.ToSnakeCase());
        return $"SELECT {Join(columns)} FROM {type.Name.ToSnakeCase()} WHERE {columnName.ToSnakeCase()} = '{value}'";
    }
    public static string LetCreate<T>(in IList<string> columnNames) => LetCreate(typeof(T), columnNames);
    public static string LetCreate(in Type type, in IList<string> columnNames) =>
        $"CREATE TABLE IF NOT EXISTS {type.Name.ToSnakeCase()}({Join(columnNames)})";
    public static string LetDelete<T>(in Type type, in string? identifyName = null) =>
        $"DELETE FROM {type.Name.ToSnakeCase()} {QueryCondition(identifyName)}";
    public static string LetDelete<T>(in string id) where T : notnull =>
        $"DELETE FROM {typeof(T).Name.ToSnakeCase()} WHERE {nameof(SQLiteBase.Id).ToSnakeCase()} = '{id}'";
    public static (string sql, DynamicParameters param) LetInsert<T>(in T entity) where T : notnull
    {
        List<string> columns = [];
        var type = entity.GetType();
        DynamicParameters parameters = new();
        foreach (var property in type.GetProperties())
        {
            var columnName = property.Name.ToSnakeCase();
            var value = property.GetValue(entity);
            if (value is not null)
            {
                columns.Add(columnName);
                parameters.Add(columnName, value);
            }
        }
        return ($"""
        INSERT INTO {type.Name.ToSnakeCase()}({Join(columns)}) VALUES ({Join(columns.Select(x => AtTag(x)))})
        """, parameters);
    }
    public static (string sql, DynamicParameters param) LetUpdate<T>(in string id, in T entity, in string identify, IEnumerable<string> filters) where T : notnull
    {
        List<string> columns = [];
        var type = entity.GetType();
        DynamicParameters parameters = new();
        parameters.Add(AtTag(identify), id);
        foreach (var property in type.GetProperties().Where(property => !filters.Any(x => x.IsMatch(property.Name))))
        {
            var columnName = property.Name.ToSnakeCase();
            var value = property.GetValue(entity);
            if (value is not null)
            {
                columns.Add(ConditionTag(columnName));
                parameters.Add(columnName, value);
            }
        }
        return ($"""
        UPDATE {type.Name.ToSnakeCase()} SET {Join(columns)} {QueryCondition(identify)}
        """, parameters);
    }
    public static string LetUniqueIndex(in string tableName, in string columnName) =>
        $"CREATE UNIQUE INDEX IF NOT EXISTS {tableName}_{columnName} ON {tableName} ({columnName});";
    public static string AtTag(in string columnName) => $"{TextExpand.Link}{columnName}";
    public static string LocalTime(in DateTime dateTime, in int timeZone, in string timeFormat) =>
        dateTime.AddHours(timeZone).ToString(timeFormat, CultureInfo.InvariantCulture);
    public static string? JoinQueries(in IEnumerable<string>? contents)
    {
        var counter = (int)default;
        if (contents is not null)
        {
            System.Runtime.CompilerServices.DefaultInterpolatedStringHandler handler = new(default, contents.Count());
            foreach (var content in contents)
            {
                handler.AppendFormatted(content);
                if (contents.Count() - counter++ is not 1) handler.AppendFormatted($";{TextExpand.Empty}");
            }
            return handler.ToStringAndClear();
        }
        return default;
    }
    public static string GetSnowflakeId() => IdAlgorithm.Next().ToString(CultureInfo.InvariantCulture);
    public static Task<SqlMapper.GridReader> MultipleQueriesAsync(in SqliteConnection connection, in IEnumerable<string>? contents)
    {
        return connection.QueryMultipleAsync(JoinQueries(contents) ?? string.Empty);
    }
    public static Task<SqlMapper.GridReader> MultipleQueriesAsync(in NpgsqlConnection connection, in IEnumerable<string>? contents)
    {
        return connection.QueryMultipleAsync(JoinQueries(contents) ?? string.Empty);
    }
    static string Join(in IEnumerable<string> columns)
    {
        var counter = (int)default;
        var formatCount = columns.Count();
        System.Runtime.CompilerServices.DefaultInterpolatedStringHandler handler = new(default, formatCount);
        foreach (var column in columns)
        {
            handler.AppendFormatted(column);
            if (formatCount - counter++ is not 1) handler.AppendFormatted(',');
        }
        return handler.ToStringAndClear();
    }
    static string ConditionTag(in string columnName) => $"{columnName.ToSnakeCase()}={AtTag(columnName)}";
    static string QueryCondition(in string? columnName) => columnName is null ? string.Empty : $"WHERE {ConditionTag(columnName)}";
    static string Sequencer => $"ORDER BY {nameof(SQLiteBase.CreateTime).ToSnakeCase()} DESC";
}