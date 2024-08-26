namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.ActivityRecords;
internal sealed class ListActivityRecordInput : NodeQueryer
{
    public string? UserId { get; init; }
};