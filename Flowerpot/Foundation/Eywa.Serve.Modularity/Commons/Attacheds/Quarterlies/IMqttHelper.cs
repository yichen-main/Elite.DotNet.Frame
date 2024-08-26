namespace Eywa.Serve.Modularity.Commons.Attacheds.Quarterlies;
public interface IMqttHelper
{
    void Build(in MqttServer server);
    void Subscribe<T>(Action<T> action) where T : struct;
    void Subscribe<T>(string topic, Action<T> action) where T : struct;
    void Subscribe(Action<(string id, string topic, string payload)> content);
    ValueTask PublishAsync<T>(T entity, CancellationToken cancellationToken = default) where T : notnull;
    bool Enabled { get; }
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class MqttHelper : IMqttHelper
{
    public void Build(in MqttServer server)
    {
        Server ??= server;
        Enabled = server.AcceptNewConnections;
    }
    public void Subscribe<T>(Action<T> action) where T : struct => Server.InterceptingPublishAsync += @event => Task.Run(async () =>
    {
        try
        {
            if (!@event.ClientId.StartCharsetExists(nameof(Eywa)))
            {
                //var topic = ReflexExpand.GetAttribute<T, TopicAttribute>();
                //if (topic is not null && topic.Name.IsExist(@event.ApplicationMessage.Topic, StringComparison.OrdinalIgnoreCase))
                //{
                //    action(@event.ApplicationMessage.PayloadSegment.GetString().ToObject<T>());
                //}
            }
        }
        catch (Exception e)
        {
            await FileLayout.OutputAsync(new()
            {
                Type = GetType(),
                Line = e.GetLine(),
                Name = nameof(Subscribe),
                Message = e.Message,
                Content = new
                {
                    @event.ClientId,
                    @event.ApplicationMessage.Topic,
                    Payload = @event.ApplicationMessage.PayloadSegment.GetString(),
                },
            }).ConfigureAwait(false);
        }
        return ValueTask.CompletedTask;
    }, @event.CancellationToken);
    public void Subscribe<T>(string topic, Action<T> action) where T : struct => Server.InterceptingPublishAsync += @event => Task.Run(async () =>
    {
        try
        {
            if (!@event.ClientId.StartCharsetExists(nameof(Eywa)) && topic.IsMatch(@event.ApplicationMessage.Topic, StringComparison.OrdinalIgnoreCase))
            {
                action(@event.ApplicationMessage.PayloadSegment.GetString().ToGenerics<T>());
            }
        }
        catch (Exception e)
        {
            await FileLayout.OutputAsync(new()
            {
                Type = GetType(),
                Line = e.GetLine(),
                Name = nameof(Subscribe),
                Message = e.Message,
                Content = new
                {
                    @event.ClientId,
                    @event.ApplicationMessage.Topic,
                    Payload = @event.ApplicationMessage.PayloadSegment.GetString(),
                },
            }).ConfigureAwait(false);
        }
        return ValueTask.CompletedTask;
    }, @event.CancellationToken);
    public void Subscribe(Action<(string id, string topic, string payload)> content) => Server.InterceptingPublishAsync += async @event =>
    {
        try
        {
            if (!@event.ClientId.StartCharsetExists(nameof(Eywa))) content(new()
            {
                id = @event.ClientId,
                topic = @event.ApplicationMessage.Topic,
                payload = @event.ApplicationMessage.PayloadSegment.GetString(),
            });
        }
        catch (Exception e)
        {
            await FileLayout.OutputAsync(new()
            {
                Type = GetType(),
                Line = e.GetLine(),
                Name = nameof(Subscribe),
                Message = e.Message,
                Content = new
                {
                    @event.ClientId,
                    @event.ApplicationMessage.Topic,
                    Payload = @event.ApplicationMessage.PayloadSegment.GetString(),
                    e.Message,
                },
            }).ConfigureAwait(false);
        }
    };
    public async ValueTask PublishAsync<T>(T entity, CancellationToken cancellationToken = default) where T : notnull
    {
        var clientId = $"{nameof(Eywa)}{Guid.NewGuid()}";
        //var attribute = ReflexExpand.GetAttribute<T, TopicAttribute>();
        try
        {
            //if (attribute is not null)
            //{
            MqttApplicationMessage content = new()
            {
                Retain = true,
                Topic = "attribute.Name",
                PayloadSegment = Encoding.UTF8.GetBytes(entity.ToJson()),
                QualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce,
            };
            await Server.InjectApplicationMessage(new InjectedMqttApplicationMessage(content)
            {
                SenderClientId = clientId,
            }, cancellationToken).ConfigureAwait(false);
            //}
        }
        catch (Exception e)
        {
            var key = $"{clientId}{e.Message}";
            if (!Histories.Contains(key, StringComparer.Ordinal))
            {
                Histories.Add(key);
                await FileLayout.OutputAsync(new()
                {
                    Type = GetType(),
                    Line = e.GetLine(),
                    Name = nameof(PublishAsync),
                    Message = e.Message,
                    Content = new
                    {
                        ClientId = clientId,
                        Topic = "attribute?.Name",
                        Payload = entity.ToJson(indented: true),
                    },
                }).ConfigureAwait(false);
            }
        }
    }
    public bool Enabled { get; private set; }
    static List<string> Histories { get; } = [];
    static MqttServer Server { get; set; } = null!;
}