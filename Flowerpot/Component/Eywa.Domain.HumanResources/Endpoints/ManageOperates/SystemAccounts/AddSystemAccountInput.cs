namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.SystemAccounts;
internal sealed class AddSystemAccountInput : NodeHeader
{
    public required string Password { get; init; }
    public sealed class Validator : AbstractValidator<AddSystemAccountInput>
    {
        public Validator(ILocalCulture culture)
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.WrongPassword));
        }
    }
}