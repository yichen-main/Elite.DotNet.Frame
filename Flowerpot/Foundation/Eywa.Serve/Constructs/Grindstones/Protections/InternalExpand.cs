namespace Eywa.Serve.Constructs.Grindstones.Protections;
internal static class InternalExpand
{
    public record ListenInfo(string Name, int Port, Action<ListenOptions>? ListenOptions);
    public static async ValueTask InitialAsync<T>(int frequency = 100)
    {
        var title = nameof(System);
#if DEBUG
        title = $"{title} [{nameof(Debug).ToUpper(CultureInfo.CurrentCulture)}]";
#endif
        Console.Title = title;
        Console.CursorVisible = default;
        await Print(GetNameplate()).PrintAsync(ConsoleColor.Yellow).ConfigureAwait(false);
        await new[]
        {
            FiggleFonts.Standard.Render($"{nameof(Eywa)} {nameof(Serve)}".Aggregate(string.Empty.PadLeft(2,  TextExpand.Empty),
            (first, second) => string.Concat(first, second, string.Empty.PadLeft(1, TextExpand.Empty)))),
            new string('*', 79), Environment.NewLine,
        }.Merge().PrintAsync().ConfigureAwait(false);
        if (!Missions.Keys.Any(x => x.IsMatch(nameof(InitialAsync)))) Missions[nameof(InitialAsync)] = Task.Run(async () =>
        {
            PeriodicTimer timer = new(TimeSpan.FromMilliseconds(frequency));
            string name = string.Empty, stroke = "/", dash = "-", backslash = "\\", or = "|";
            while (await timer.WaitForNextTickAsync(default).ConfigureAwait(false))
            {
                Console.SetCursorPosition(default, Console.CursorTop);
                Console.Write(name switch
                {
                    var x when x.IsMatch(or) => name = stroke,
                    var x when x.IsMatch(stroke) => name = dash,
                    var x when x.IsMatch(backslash) => name = or,
                    _ => name = backslash,
                });
                if (!Display)
                {
                    Console.SetCursorPosition(default, Console.CursorTop);
                    timer.Dispose();
                }
            }
            Console.SetCursorPosition(default, Console.CursorTop);
        }, default);
        static string Print(in (string tag, string content)[] menus)
        {
            List<string> lines = [];
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            var space = $"{TextExpand.Empty}{TextExpand.Empty}{TextExpand.Empty}";
            foreach (var (key, content) in menus) lines.Add($"{key,16}{space}=>{space}{content,-10}{Environment.NewLine}");
            return ((string[])[.. lines]).Merge();
        }
        static (string tag, string content)[] GetNameplate() => [
            ("Host name", Dns.GetHostName()),
            ("User name", Environment.UserName),
            (".NET SDK", Environment.Version.ToString()),
            ("Internet", NetworkInterface.GetIsNetworkAvailable().ToString()),
            ("Language tag", Thread.CurrentThread.CurrentCulture.IetfLanguageTag),
            ("Language name", Thread.CurrentThread.CurrentCulture.DisplayName),
            ("IPv4 physical", NetworkInterfaceType.Ethernet.GetLocalIPv4().FirstOrDefault() ?? string.Empty),
            ("IPv4 wireless", NetworkInterfaceType.Wireless80211.GetLocalIPv4().FirstOrDefault() ?? string.Empty),
            ("Project name", FileLayout.GetProjectName<T>()),
            ("OS version", Environment.OSVersion.ToString()),
        ];
    }
    public static void Listening(this KestrelServerOptions options, IEnumerable<ListenInfo> endpoints)
    {
        foreach (var endpoint in endpoints)
        {
            if (!IPAddress.Loopback.VerifyPort(port: endpoint.Port))
            {
                switch (endpoint.ListenOptions)
                {
                    case not null:
                        options.ListenAnyIP(endpoint.Port, endpoint.ListenOptions);
                        break;

                    default:
                        options.ListenAnyIP(endpoint.Port);
                        break;
                }
                ListenBanners.Add(string.Create(CultureInfo.InvariantCulture, $"{endpoint.Name}: {endpoint.Port}"));
            }
            else PortExisted = true;
        }
    }
    public static IServiceCollection AddCharger(this IServiceCollection services)
    {
        return services.AddTransient<IEditionRepository>(x => new StandardEdition());
    }
    public static void AddEnvironmentSettings(this WebApplicationBuilder builder)
    {
        builder.Logging.AddFilter<ConsoleLoggerProvider>(nameof(Microsoft), LogLevel.Error);
        builder.Logging.AddFilter<ConsoleLoggerProvider>(nameof(FastEndpoints), LogLevel.Error);
        builder.Services.Configure<HostInformation>(builder.Configuration.GetSection(nameof(HostInformation)));
        builder.Services.Configure<LocalizationSettings>(builder.Configuration.GetSection(nameof(LocalizationSettings)));
        builder.Services.Configure<RequestLocalizationOptions>(x =>
        {
            List<CultureInfo> cultureInfos = [];
            var settings = builder.Configuration.GetSection(nameof(LocalizationSettings)).Get<LocalizationSettings>();
            if (settings is not null)
            {
                cultureInfos.AddRange(settings.SupportedCultures.Select(x => new CultureInfo(x)));
                x.DefaultRequestCulture = new(settings.DefaultCulture);
                x.SupportedCultures = cultureInfos;
                x.SupportedUICultures = cultureInfos;
                x.RequestCultureProviders.Insert(default, new AcceptLanguageHeaderRequestCultureProvider());
            }
        });
    }
    public static WebApplication UseTogether(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseRouting();
        app.UseCors();
        app.UseRateLimiter();
        app.UseAuthorization();
        app.UseRequestLocalization();
        return app;
    }
    public static WebApplicationBuilder UseTogether(this WebApplicationBuilder builder)
    {
        builder.Services.AddSoapCore();
        builder.Services.AddHttpClient();
        builder.Services.AddProblemDetails();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddEndpointsApiExplorer();
        builder.Host.ConfigureHostOptions(x =>
        {
            x.ServicesStopConcurrently = true;
            x.ServicesStartConcurrently = true;
            x.ShutdownTimeout = TimeSpan.FromSeconds(30);
        }).UseSystemd();
        builder.Services.ConfigureHttpJsonOptions(x =>
        {
            x.SerializerOptions.Converters.Add(new DateConvert());
            x.SerializerOptions.Converters.Add(new EnumConvert());
            x.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        builder.Services.AddControllers(x =>
        {
            x.ReturnHttpNotAcceptable = true;
        }).ConfigureApiBehaviorOptions(x =>
        {
            x.SuppressModelStateInvalidFilter = default;
        }).AddMvcOptions(x => x.Conventions.Add(new ModelConvention())).AddControllersAsServices();
        builder.Services.AddExceptionHandler<ExceptionMiddle>();
        return builder;
    }
    public static void AddValidator(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddDefaultPolicy(x =>
        x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("*")));
        builder.Services.AddAuthentication(nameof(Eywa)).AddScheme<AuthOption, AuthHandler>(nameof(Eywa), option =>
        {

        });
    }
    public static async ValueTask CloseMarqueeAsync()
    {
        Display = default;
        if (Missions.TryGetValue(nameof(InitialAsync), out var mission)) await mission.ConfigureAwait(false);
    }
    public static bool PortExisted { get; private set; }
    public static IList<string> ListenBanners { get; private set; } = [];
    public static ConcurrentDictionary<string, Dictionary<string, string>> Dialects { get; } = [];
    static Dictionary<string, Task> Missions { get; } = new Dictionary<string, Task>(StringComparer.Ordinal);
    static bool Display { get; set; } = true;
}