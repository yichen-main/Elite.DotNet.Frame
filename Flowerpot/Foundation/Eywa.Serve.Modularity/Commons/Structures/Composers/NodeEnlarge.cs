namespace Eywa.Serve.Modularity.Commons.Structures.Composers;
public abstract class NodeEnlarge<THandler, TRequest>(RolePolicy role = RolePolicy.Viewer) : Endpoint<TRequest> where TRequest : notnull
{
    protected const string RemovChecklistTag = "checklists";
    protected const string RefreshTokenTag = "refresh-token";
    protected readonly record struct ClaimPoint
    {
        public required string Id { get; init; }
        public required string UserName { get; init; }
    }
    protected readonly record struct OutputToken
    {
        public required string AccessToken { get; init; }
        public required int ExpiresIn { get; init; }
        public required string TokenType { get; init; }
        public required string RefreshToken { get; init; }
    }
    protected void RequiredConfiguration()
    {
        RoutingSettings();
        DontCatchExceptions();
        void RoutingSettings()
        {
            switch (Type.Name)
            {
                case var item when item.IsFuzzy("List"):
                    Get(Route);
                    break;

                case var item when item.IsFuzzy("Get"):
                    Get($"{Route}/{{id}}");
                    break;

                case var item when item.IsFuzzy("Enum"):
                    Get($"{Route}/{Type.Name
                        .Replace("Enum", string.Empty, StringComparison.Ordinal)
                        .Replace("Vehicle", string.Empty, StringComparison.Ordinal).LetConvertPath()}");
                    break;

                case var item when item.IsFuzzy("Export"):
                    Get($"{Route}/exports");
                    break;

                case var item when item.IsFuzzy("View"):
                    Get($"{Route}/remove-displays");
                    break;

                case var item when item.IsFuzzy("Add"):
                    Post(Route);
                    break;

                case var item when item.IsFuzzy("Put"):
                    Put(Route);
                    break;

                case var item when item.IsFuzzy("Import"):
                    Put($"{Route}/imports");
                    break;

                case var item when item.IsFuzzy("Clear"):
                    Delete(Route);
                    break;

                case var item when item.IsFuzzy("Del"):
                    Delete($"{Route}/{{id}}");
                    break;
            }
        }
    }
    protected string BearerToken(in ClaimPoint claimPoint)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurePicker.ModuleProfile.HTTPAuthentication.Secret));
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: [
            new Claim(ClaimTypes.NameIdentifier, claimPoint.Id),
            new Claim(ClaimTypes.Name, claimPoint.UserName),
        ],
        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
        expires: DateTime.UtcNow.AddSeconds(ConfigurePicker.ModuleProfile.HTTPAuthentication.ExpirySeconds)));
    }
    protected void SetRefreshTokenCookies(in string value) =>
    HttpContext.Response.Cookies.Append(RefreshTokenTag, value, new CookieOptions
    {
        HttpOnly = true,
        Expires = DateTimeOffset.UtcNow.AddDays(ConfigurePicker.ModuleProfile.HTTPAuthentication.ExpirationDays),
    });
    protected void AddHeader(in string key, in string value) => HttpContext.Response.Headers.Append(key, value);
    protected DateTime ParseDateTimeFormat(in string dateTime, in string format)
    {
        try
        {
            return DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return default;
        }
    }
    protected (DateTime startTime, DateTime endTime) SplitDate(in string? interval)
    {
        DateTime startTime = default, endTime = default;
        if (interval is null) return (startTime, endTime);
        try
        {
            var dates = interval.Split(['@']);
            if (dates.Length is 1) return (startTime, endTime);
            startTime = DateTime.ParseExact(dates[0], TextExpand.DefaultDateTime, CultureInfo.InvariantCulture);
            endTime = DateTime.ParseExact(dates[1], TextExpand.DefaultDateTime, CultureInfo.InvariantCulture);
            return (startTime, endTime);
        }
        catch (Exception)
        {
            return (startTime, endTime);
        }
    }
    protected string? GetUserId() => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    protected string GetUserName()
    {
        var claim = HttpContext.User.FindFirst(ClaimTypes.Name);
        return claim is not null ? claim.Value : string.Empty;
    }
    protected string? GetCookies(in string key) => HttpContext.Request.Cookies[key];
    protected OutputToken GetTokenInfo(in ClaimPoint point, in string refreshToken) => new()
    {
        AccessToken = BearerToken(point),
        ExpiresIn = ConfigurePicker.ModuleProfile.HTTPAuthentication.ExpirySeconds,
        TokenType = JwtBearerDefaults.AuthenticationScheme,
        RefreshToken = refreshToken,
    };
    public void MakeException(in Exception e, in (string tag, string dialect)[]? contents = default)
    {
        var message = e.Message;
        foreach (var (tag, dialect) in contents ?? [])
        {
            if (e.Message.IsFuzzy(tag.ToSnakeCase())) message = dialect;
        }
        throw new Exception(message, e);
    }
    protected void Pagination<T>(in PageContents<T> contents, in IEnumerable<(string key, string? value)> queries)
    {
        var previousLink = Link(Upper(contents.CurrentPage), contents.PageSize, queries);
        var nextLink = Link(Down(contents.CurrentPage), contents.PageSize, queries);
        var firstLink = Link(1, contents.PageSize, queries);
        var lastLink = Link(contents.TotalPages, contents.PageSize, queries);
        AddHeader(HeaderName.Pagination, new
        {
            contents.PageSize,
            contents.TotalPages,
            contents.TotalEntries,
            contents.CurrentPage,
            PreviousLink = contents.CurrentPage > 1 ? previousLink : firstLink,
            NextLink = contents.CurrentPage < contents.TotalPages ? nextLink : lastLink,
            FirstLink = firstLink,
            LastLink = lastLink,
        }.ToJson(indented: false));
        int Down(in int count) => count + 1;
        int Upper(in int count) => count is 1 ? 1 : count - 1;
        string Link(in int pageCount, in int pageSize, in IEnumerable<(string key, string? value)> queries)
        {
            UriBuilder uriBuilder = new($"{BaseURL}{Route}");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[nameof(pageCount)] = HttpUtility.UrlEncode(pageCount.ToString(CultureInfo.InvariantCulture));
            query[nameof(pageSize)] = HttpUtility.UrlEncode(pageSize.ToString(CultureInfo.InvariantCulture));
            foreach (var (key, value) in queries)
            {
                if (!string.IsNullOrEmpty(value)) query.Add(key.FirstCharLowercase(), HttpUtility.UrlEncode(value));
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }
    protected Task ExecuteAsync(Func<NpgsqlConnection, Task<SqlMapper.GridReader>, Task> options, IEnumerable<string>? fields = default)
    {
        return NpgsqlHelper.ConnectAsync(connection => options(connection, TableLayout.MultipleQueriesAsync(connection, fields)));
    }
    protected Task ExecuteAsync(Func<NpgsqlConnection, Task<SqlMapper.GridReader>, NpgsqlTransaction, Task> options, CancellationToken ct, IEnumerable<string>? fields = default)
    {
        return NpgsqlHelper.TransactionAsync((connection, transaction) => options(connection, TableLayout.MultipleQueriesAsync(connection, fields), transaction), ct);
    }
    protected void VerifyRootMember()
    {
        if (!GetUserId().IsMatch(ConfigurePicker.ModuleProfile.HTTPAuthentication.AccountName))
            throw new Exception(LocalCulture.Link(AccountAccessFlag.IncorrectAccessPermissions));
    }
    protected async ValueTask VerifyHumanMemberAsync()
    {
        var membersAsync = GetMembersAsync(new HumanMemberQueryImport());
        var moduleName = LocalCulture.Link(HumanResourcesFlag.ModuleName);
        var templet = LocalCulture.Link(AccountAccessFlag.IncorrectModulePermissions);
        var members = await membersAsync.ConfigureAwait(false);
        var member = members.FirstOrDefault(x => x.UserId.IsMatch(GetUserId()));
        switch (member)
        {
            case var x when x is { UserId: not null }:
                Verify(member.RolePolicy);
                break;

            default:
                throw new Exception(string.Format($"{templet}", moduleName));
        }
    }
    protected async ValueTask VerifyEquipmentMemberAsync()
    {
        var membersAsync = GetMembersAsync(new EquipmentMemberQueryImport());
        var moduleName = LocalCulture.Link(ProductionControlFlag.ModuleName);
        var templet = LocalCulture.Link(AccountAccessFlag.IncorrectModulePermissions);
        var members = await membersAsync.ConfigureAwait(false);
        var member = members.FirstOrDefault(x => x.UserId.IsMatch(GetUserId()));
        switch (member)
        {
            case var x when x is { UserId: not null }:
                Verify(member.RolePolicy);
                break;

            default:
                throw new Exception(string.Format($"{templet}", moduleName));
        }
    }
    protected async ValueTask VerifyProductionMemberAsync()
    {
        var membersAsync = GetMembersAsync(new ProductionMemberQueryImport());
        var moduleName = LocalCulture.Link(ProductionControlFlag.ModuleName);
        var templet = LocalCulture.Link(AccountAccessFlag.IncorrectModulePermissions);
        var members = await membersAsync.ConfigureAwait(false);
        var member = members.FirstOrDefault(x => x.UserId.IsMatch(GetUserId()));
        switch (member)
        {
            case var x when x is { UserId: not null }:
                Verify(member.RolePolicy);
                break;

            default:
                throw new Exception(string.Format($"{templet}", moduleName));
        }
    }
    protected Task<IEnumerable<HumanMemberQueryOutput>> GetMembersAsync(in HumanMemberQueryImport import) =>
        Mediator.Send(import, HttpContext.RequestAborted);
    protected Task<IEnumerable<EquipmentMemberQueryOutput>> GetMembersAsync(in EquipmentMemberQueryImport import) =>
        Mediator.Send(import, HttpContext.RequestAborted);
    protected Task<IEnumerable<ProductionMemberQueryOutput>> GetMembersAsync(in ProductionMemberQueryImport import) =>
        Mediator.Send(import, HttpContext.RequestAborted);
    void Verify(in RolePolicy policy)
    {
        switch (role)
        {
            case RolePolicy.Owner:
                if (policy > RolePolicy.Owner) throw new Exception(LocalCulture.Link(AccountAccessFlag.IncorrectAccessPermissions));
                break;

            case RolePolicy.Editor:
                if (policy > RolePolicy.Editor) throw new Exception(LocalCulture.Link(AccountAccessFlag.IncorrectAccessPermissions));
                break;

            case RolePolicy.Viewer:
                if (policy > RolePolicy.Viewer) throw new Exception(LocalCulture.Link(AccountAccessFlag.IncorrectAccessPermissions));
                break;
        }
    }
    string Route
    {
        get
        {
            var paths = Type.Namespace!
                .Replace("Domain", string.Empty, StringComparison.Ordinal)
                .Replace("Endpoints", string.Empty, StringComparison.Ordinal)
                .Replace("Application", string.Empty, StringComparison.Ordinal)
                .Replace("..", ".", StringComparison.Ordinal)
                .TrimStart('.').TrimEnd('.').Split(".");
            StringBuilder builder = new();
            for (var i = default(int); i < paths.Length; i++)
            {
                var tag = paths[i].LetConvertPath();
                if (i == paths.Length - 1) builder.Append(tag);
                else builder.Append($"{tag}/");
            }
            return builder.ToString();
        }
    }
    Type Type { get; } = typeof(THandler);
    public required IMediator Mediator { get; init; }
    public required INpgsqlHelper NpgsqlHelper { get; init; }
    public required ILocalCulture LocalCulture { get; init; }
    public required IConfigurePicker ConfigurePicker { get; init; }
    public required ICiphertextPolicy CiphertextPolicy { get; init; }
}