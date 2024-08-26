namespace Eywa.Serve.Modularity.Transforms.GraphicLinks.ProductWorkshops;
public interface IManufacturerLink
{
    void Push(in string topic, in LinkTagInput input);
    ValueTask PushAsync<T>(in IEnumerable<T> messages);
    void AddOrUpdate(in string topic, in string machineNo, in string dataNo);
    bool GetStatus(in string machineNo, in string dataNo);
    T GetData<T>(in string machineNo, in string dataNo) where T : INumber<T>;
    (T value, DateTime time) GetValue<T>(in string machineNo, in string dataNo) where T : INumber<T>;
    FrozenDictionary<(string machineNo, string dataNo), (string value, DateTime time)> GetFrozen();
    ConcurrentDictionary<string, (string machineNo, string dataNo)> LinkProfiles { get; }
    ConcurrentDictionary<(string machineNo, string dataNo), (string value, DateTime time)> MachineDatas { get; }
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class ManufacturerLink : IManufacturerLink
{
    public void Push(in string topic, in LinkTagInput input)
    {
        var (machineNo, dataNo) = LinkProfiles[topic];
        MachineDatas[(machineNo, dataNo)] = (input.DataValue, DateTime.UtcNow);
    }
    public ValueTask PushAsync<T>(in IEnumerable<T> messages) => TopicEventSender.SendAsync(typeof(T).Name, messages);
    public void AddOrUpdate(in string topic, in string machineNo, in string dataNo) => LinkProfiles[topic] = (machineNo, dataNo);
    public bool GetStatus(in string machineNo, in string dataNo) => GetValue<int>(machineNo, dataNo).value is 1;
    public T GetData<T>(in string machineNo, in string dataNo) where T : INumber<T> => GetValue<T>(machineNo, dataNo).value;
    public (T value, DateTime time) GetValue<T>(in string machineNo, in string dataNo) where T : INumber<T>
    {
        return MachineDatas.TryGetValue((machineNo, dataNo), out var value) && T.TryParse(value.value, provider: null, out T? result) ?
            (result, value.time) : (Activator.CreateInstance<T>(), default);
    }
    public FrozenDictionary<(string machineNo, string dataNo), (string value, DateTime time)> GetFrozen() => MachineDatas.ToFrozenDictionary();
    public ConcurrentDictionary<string, (string machineNo, string dataNo)> LinkProfiles { get; } = new(StringComparer.Ordinal);
    public ConcurrentDictionary<(string machineNo, string dataNo), (string value, DateTime time)> MachineDatas { get; } = new();
    public required ITopicEventSender TopicEventSender { get; init; }
}