namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountRegisters;
internal sealed class ListAccountRegisterVehicle : NodeEnlarge<ListAccountRegisterVehicle, ListAccountRegisterInput>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(ListAccountRegisterInput req, CancellationToken ct)
    {
        VerifyRootMember();
        List<GetAccountRegisterVehicle.Output> outputs = [];
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            var reader = await readerAsync.ConfigureAwait(false);
            var users = await reader.ReadAsync<HumanUser>().ConfigureAwait(false);
            foreach (var user in users)
            {
                outputs.Add(new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserNo = user.UserNo,
                    UserName = user.UserName,
                    Disable = user.Disable,
                    CreateTime = TableLayout.LocalTime(user.CreateTime, req.TimeZone, req.TimeFormat),
                });
            }
            Pagination(new PageContents<GetAccountRegisterVehicle.Output>(outputs, req.PageCount, req.PageSize),
            [
                (nameof(req.Search), req.Search),
            ]);
            await SendAsync(outputs, cancellation: ct).ConfigureAwait(false);
        }, [
            TableLayout.LetSelect<HumanUser>(),
        ]);
    }
}