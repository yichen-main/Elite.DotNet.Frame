namespace Eywa.Serve.Constructs.Foundations.Scaffolders;
internal sealed class StandardEdition : IEditionRepository
{
    public WebApplicationBuilder Initialize(Assembly? assembly)
    {
        var builder = MiddleHost.Create();
        builder.Services.AddControllers();
        builder.Services.AddLocalization();
        builder.Services.AddSingleton<IStringLocalizerFactory>(x => new DialectFactory());
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            if (assembly is not null)
            {
                foreach (var type in assembly.GetTypes().Where(x =>
                typeof(BaseModule).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)) LoadModule(type);
            }
            void LoadModule(in Type moduleType)
            {
                if (LoadedModules.Contains(moduleType)) return;
                var attribute = moduleType.GetCustomAttribute<DependsOnAttribute>();
                if (attribute is not null)
                {
                    foreach (var dependency in attribute.Dependencies) LoadModule(dependency);
                }
                var module = Activator.CreateInstance(moduleType) as BaseModule;
                if (module is not null)
                {
                    containerBuilder.RegisterModule(module);
                    LoadedModules.Add(moduleType);
                }
            }
        });
        builder.AddEnvironmentSettings();
        builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
        return new FiberRepository(builder, new TubeRepository(builder)).Add(x =>
        {
            var info = x.Configuration.GetSection(nameof(HostInformation)).Get<HostInformation>();
            x.Services.AddRateLimiter(x =>
            {
                x.AddFixedWindowLimiter(nameof(Eywa), x =>
                {
                    x.QueueLimit = 2;
                    x.PermitLimit = 4;
                    x.Window = TimeSpan.FromSeconds(10);
                    x.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
                x.RejectionStatusCode = 429;
            });
            x.AddValidator();
        });
    }
    public WebApplicationBuilder Add(in WebApplicationBuilder builder, in Action<KestrelServerOptions> options)
    {
        builder.UseTogether();
        builder.WebHost.UseKestrel(options).ConfigureLogging(x =>
        {
            x.ClearProviders();
            x.SetMinimumLevel(LogLevel.Critical);
        });
        return builder;
    }
    public WebApplication Add(in WebApplicationBuilder builder, Action<WebApplication, IEndpointRouteBuilder> options)
    {
        var app = builder.Build().UseTogether();
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2),
        });
        app.UseEndpoints(x => options(app, x));
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        });
        return app;
    }
    List<Type> LoadedModules { get; set; } = [];
}