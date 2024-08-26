namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.CompanyPositions;
internal sealed class AddCompanyPositionVehicle : NodeEnlarge<AddCompanyPositionVehicle, AddCompanyPositionInput>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(AddCompanyPositionInput req, CancellationToken ct)
    {
        VerifyRootMember();
        return ExecuteAsync(async (connection, readerAsync) =>
        {
            try
            {
                var userName = GetUserName();
                var id = TableLayout.GetSnowflakeId();
                await Validator.LeachAsync(req).ConfigureAwait(false);
                await NpgsqlHelper.InsertAsync(connection, new HumanUserPosition
                {
                    Id = id,
                    PositionNo = req.Body.PositionNo,
                    PositionName = req.Body.PositionName,
                    Creator = userName,
                    Modifier = userName,
                    ModifyTime = DateTime.UtcNow,
                }, ct).ConfigureAwait(false);
                await SendCreatedAtAsync<GetCompanyPositionVehicle>(new
                {
                    id,
                }, default, cancellation: ct).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                MakeException(e, [
                    (HumanUserPosition.PositionNoIndex, LocalCulture.Link(HumanResourcesFlag.PositionNoIndex)),
                ]);
            }
        });
    }
    public required IValidator<AddCompanyPositionInput> Validator { get; init; }
}