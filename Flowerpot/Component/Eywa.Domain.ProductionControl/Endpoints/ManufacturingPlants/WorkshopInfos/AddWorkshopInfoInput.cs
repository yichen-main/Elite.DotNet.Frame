namespace Eywa.Domain.ProductionControl.Endpoints.ManufacturingPlants.WorkshopInfos;
internal sealed class AddWorkshopInfoInput : NodeHeader
{
    [FromBody] public required RawBody Body { get; init; }
    public sealed class RawBody
    {
        public required string GroupNo { get; init; }
        public required string GroupName { get; init; }
        public sealed class Validator : AbstractValidator<AddWorkshopInfoInput>
        {
            public Validator(ILocalCulture culture)
            {
                RuleFor(x => x.Body.GroupNo).NotEmpty().WithMessage(culture.Link(ProductionControlFlag.GroupNoIsRequired));
                RuleFor(x => x.Body.GroupName).NotEmpty().WithMessage(culture.Link(ProductionControlFlag.GroupNameIsRequired));
            }
        }
    }
}