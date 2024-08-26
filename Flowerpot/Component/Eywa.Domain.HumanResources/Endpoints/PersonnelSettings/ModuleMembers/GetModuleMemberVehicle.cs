namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class GetModuleMemberVehicle : NodeEnlarge<GetModuleMemberVehicle, NodeIdentify>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(NodeIdentify req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            await VerifyHumanMemberAsync().ConfigureAwait(false);
            var reader = await readerAsync.ConfigureAwait(false);
            var users = await reader.ReadAsync<HumanUser>().ConfigureAwait(false);
            var module = await reader.ReadFirstOrDefaultAsync<HumanUserModule>().ConfigureAwait(false)
            ?? throw new Exception(LocalCulture.Link(HumanResourcesFlag.ModuleIdDoesNotExist));
            var user = users.First(x => x.Id.IsMatch(module.UserId));
            await SendAsync(new Output
            {
                Id = module.Id,
                Disable = module.Disable,
                DomainType = module.DomainType,
                DomainName = LocalCulture.Link(module.DomainType),
                AccessName = LocalCulture.Link(module.RolePolicy),
                UserId = user.Id,
                Email = user.Email,
                UserNo = user.UserNo,
                UserName = user.UserName,
                Creator = module.Creator,
                CreateTime = TableLayout.LocalTime(module.CreateTime, req.TimeZone, req.TimeFormat),
                Modifier = module.Modifier,
                ModifyTime = TableLayout.LocalTime(module.ModifyTime, req.TimeZone, req.TimeFormat),
            }, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(),
            TableLayout.LetSelect<HumanUserModule>(req.Id),
        ]);
    }
    public readonly record struct Output
    {
        public required string Id { get; init; }
        public required bool Disable { get; init; }
        public required DomainType DomainType { get; init; }
        public required string DomainName { get; init; }
        public required string AccessName { get; init; }
        public required string UserId { get; init; }
        public required string Email { get; init; }
        public required string UserNo { get; init; }
        public required string UserName { get; init; }
        public required string Creator { get; init; }
        public required string CreateTime { get; init; }
        public required string Modifier { get; init; }
        public required string ModifyTime { get; init; }
    }
}