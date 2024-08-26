namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class AddModuleMemberInput : NodeHeader
{
    [FromBody] public required RawBody Body { get; init; }
    public sealed class RawBody
    {
        public required string UserId { get; init; }
        public required bool Disable { get; init; }
        public RolePolicy AccessType { get; init; }
        public sealed class Validator : AbstractValidator<AddModuleMemberInput>
        {
            public Validator(ILocalCulture culture)
            {
                RuleFor(x => x.Body.UserId).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.UserIdIsRequired));
                RuleFor(x => x.Body.AccessType)
                    .Must(x => Enum.IsDefined(typeof(RolePolicy), x))
                    .WithMessage(culture.Link(HumanResourcesFlag.RolePolicyMismatch))
                    .Must(x => x is RolePolicy.Editor || x is RolePolicy.Viewer)
                    .WithMessage(culture.Link(HumanResourcesFlag.AccessTypeMismatch));
            }
        }
    }
}