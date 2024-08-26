namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountRegisters;
internal sealed class PutAccountRegisterInput : NodeHeader
{
    [FromBody] public required RawBody Body { get; init; }
    public sealed class RawBody
    {
        public required string Id { get; init; }
        public required string Email { get; init; }
        public required string UserNo { get; init; }
        public required string UserName { get; init; }
        public required bool Disable { get; init; }
        public sealed class Validator : AbstractValidator<PutAccountRegisterInput>
        {
            public Validator(ILocalCulture culture)
            {
                RuleFor(x => x.Body.Id).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.UserIdIsRequired));
                RuleFor(x => x.Body.Email).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.EmailIsRequired))
                    .EmailAddress().WithMessage(culture.Link(HumanResourcesFlag.EmailFormatError));
                RuleFor(x => x.Body.UserNo).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.UserNoIsRequired));
                RuleFor(x => x.Body.UserName).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.UserNameIsRequired));
            }
        }
    }
}