namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.TokenExtensions;
internal sealed class AddTokenExtensionInput : NodeHeader
{
    public required string NameIdentifier { get; init; }
    public required string RefreshToken { get; init; }
    public sealed class Validator : AbstractValidator<AddTokenExtensionInput>
    {
        public Validator(ILocalCulture culture)
        {
            RuleFor(x => x.NameIdentifier).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.NameIdentifierIsRequired));
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.RefreshTokenIsRequired));
        }
    }
}