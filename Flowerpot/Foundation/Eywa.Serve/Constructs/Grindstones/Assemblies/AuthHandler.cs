namespace Eywa.Serve.Constructs.Grindstones.Assemblies;
internal sealed class AuthHandler(UrlEncoder url, ILoggerFactory logger, IOptionsMonitor<AuthOption> option)
    : AuthenticationHandler<AuthOption>(option, logger, url)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (Request.Headers.TryGetValue(HeaderName.Authorization, out var value))
            {
                var token = GetAccessToken(value);
                if (token.ValidTo >= DateTime.UtcNow)
                {
                    ClaimsIdentity claimsIdentity = new(token.Claims, nameof(Eywa));
                    ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                    AuthenticationTicket authTicket = new(claimsPrincipal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(authTicket));
                }
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        catch (Exception e)
        {
            return Task.FromResult(AuthenticateResult.Fail(e));
        }
    }
    JwtSecurityToken GetAccessToken(in StringValues value)
    {
        var names = value.ToString().Split(' ');
        return names.Length != 2 || string.IsNullOrEmpty(names[1]) || names[1].IsMatch("null") || !names[0].IsMatch(
            JwtBearerDefaults.AuthenticationScheme) ? throw new Exception() : new JwtSecurityTokenHandler().ReadJwtToken(names[1]);
    }
}