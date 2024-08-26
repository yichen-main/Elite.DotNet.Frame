namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountRegisters;
internal sealed class GetAccountRegisterVehicle : NodeEnlarge<GetAccountRegisterVehicle, NodeIdentify>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(NodeIdentify req, CancellationToken ct)
    {
        VerifyRootMember();
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            var reader = await readerAsync.ConfigureAwait(false);
            var user = await reader.ReadFirstAsync<HumanUser>().ConfigureAwait(false);
            await SendAsync(new Output
            {
                Id = user.Id,
                Email = user.Email,
                UserNo = user.UserNo,
                UserName = user.UserName,
                Disable = user.Disable,
                CreateTime = TableLayout.LocalTime(user.CreateTime, req.TimeZone, req.TimeFormat),
            }, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(req.Id),
        ]);
    }
    public readonly record struct Output
    {
        public required string Id { get; init; }
        public required string Email { get; init; }
        public required string UserNo { get; init; }
        public required string UserName { get; init; }
        public required bool Disable { get; init; }
        public required string CreateTime { get; init; }
    }
}