namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleSupervisors;
internal sealed class PutModuleSupervisorInput : NodeHeader
{
    [FromBody] public required RawBody Body { get; init; }
    public sealed class RawBody
    {
        public required string Id { get; init; }
        public required string UserId { get; init; }
        public required bool Disable { get; init; }
        public required DomainType DomainType { get; init; }
        public sealed class Validator : AbstractValidator<PutModuleSupervisorInput>
        {
            public Validator(ILocalCulture culture)
            {
                RuleFor(x => x.Body.Id).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.ModuleIdIsRequired));
                RuleFor(x => x.Body.UserId).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.UserIdIsRequired));
                RuleFor(x => x.Body.DomainType).Must(type => BeforeExpand.GetModules().Any(x => x == type))
                 .WithMessage(culture.Link(HumanResourcesFlag.DomainTypeMismatch));
            }
        }
    }
}