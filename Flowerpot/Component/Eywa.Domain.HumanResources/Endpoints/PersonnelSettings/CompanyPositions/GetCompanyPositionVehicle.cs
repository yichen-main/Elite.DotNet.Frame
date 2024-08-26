namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.CompanyPositions;
internal sealed class GetCompanyPositionVehicle : NodeEnlarge<GetCompanyPositionVehicle, NodeIdentify>
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
            var position = await reader.ReadFirstAsync<HumanUserPosition>().ConfigureAwait(false);
            await SendAsync(new Output
            {
                Id = position.Id,
                PositionNo = position.PositionNo,
                PositionName = position.PositionName,
                Creator = position.Creator,
                CreateTime = TableLayout.LocalTime(position.CreateTime, req.TimeZone, req.TimeFormat),
                Modifier = position.Modifier,
                ModifyTime = TableLayout.LocalTime(position.ModifyTime, req.TimeZone, req.TimeFormat),
            }, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUserPosition>(req.Id),
        ]);
    }
    public readonly record struct Output
    {
        public required string Id { get; init; }
        public required string PositionNo { get; init; }
        public required string PositionName { get; init; }
        public required string Creator { get; init; }
        public required string CreateTime { get; init; }
        public required string Modifier { get; init; }
        public required string ModifyTime { get; init; }
    }
}