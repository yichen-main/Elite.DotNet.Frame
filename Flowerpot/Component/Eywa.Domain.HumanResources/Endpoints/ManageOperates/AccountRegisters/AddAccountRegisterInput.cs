namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountRegisters;
internal sealed class AddAccountRegisterInput : NodeHeader
{
    public required string Email { get; init; }
    public required string UserNo { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public sealed class Validator : AbstractValidator<AddAccountRegisterInput>
    {
        public Validator(ILocalCulture culture)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.EmailIsRequired))
                .EmailAddress().WithMessage(culture.Link(HumanResourcesFlag.EmailFormatError));
            RuleFor(x => x.UserNo).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.UserNoIsRequired));
            RuleFor(x => x.UserName).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.UserNameIsRequired));
            RuleFor(x => x.Password).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.PasswordIsRequired));
        }
    }
}