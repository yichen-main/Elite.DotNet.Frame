namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.ActivityRecords;
internal sealed class ListActivityRecordVehicle : NodeEnlarge<ListActivityRecordVehicle, ListActivityRecordInput>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(ListActivityRecordInput req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            List<Output> outputs = [];
            await VerifyHumanMemberAsync().ConfigureAwait(false);
            var reader = await readerAsync.ConfigureAwait(false);
            var users = await reader.ReadAsync<HumanUser>().ConfigureAwait(false);
            var records = await reader.ReadAsync<HumanUserRecord>().ConfigureAwait(false);
            foreach (var record in records)
            {
                var user = users.First(x => x.Id.IsMatch(record.UserId));
                outputs.Add(new()
                {
                    Id = record.Id,
                    StatusName = LocalCulture.Link(record.LoginStatus),
                    UserId = user.Id,
                    Email = user.Email,
                    UserNo = user.UserNo,
                    UserName = user.UserName,
                    CreateTime = TableLayout.LocalTime(record.CreateTime, req.TimeZone, req.TimeFormat),
                });
            }
            var results = req.UserId == default ? outputs : outputs.Where(x => x.UserId.IsMatch(req.UserId));
            Pagination(new PageContents<Output>(results, req.PageCount, req.PageSize),
            [
                (nameof(req.Search), req.Search),
                (nameof(req.UserId), req.UserId),
            ]);
            await SendAsync(results, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(),
            TableLayout.LetSelect<HumanUserRecord>(),
        ]);
    }
    public readonly struct Output
    {
        public required string Id { get; init; }
        public required string StatusName { get; init; }
        public required string UserId { get; init; }
        public required string Email { get; init; }
        public required string UserNo { get; init; }
        public required string UserName { get; init; }
        public required string CreateTime { get; init; }
    }
}