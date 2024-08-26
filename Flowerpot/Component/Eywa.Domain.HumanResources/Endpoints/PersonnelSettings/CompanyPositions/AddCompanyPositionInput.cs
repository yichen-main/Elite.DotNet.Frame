namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.CompanyPositions;
internal sealed class AddCompanyPositionInput : NodeHeader
{
    [FromBody] public required RawBody Body { get; init; }
    public sealed class RawBody
    {
        public required string PositionNo { get; init; }
        public required string PositionName { get; init; }
        public sealed class Validator : AbstractValidator<AddCompanyPositionInput>
        {
            public Validator(ILocalCulture culture)
            {
                RuleFor(x => x.Body.PositionNo).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.PositionNoIsRequired));
                RuleFor(x => x.Body.PositionName).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.PositionNameIsRequired));
            }
        }
    }
}