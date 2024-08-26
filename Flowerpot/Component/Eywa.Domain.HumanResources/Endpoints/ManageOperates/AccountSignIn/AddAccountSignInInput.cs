﻿namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountSignIn;
internal sealed class AddAccountSignInInput : NodeHeader
{
    public required string Account { get; init; }
    public required string Password { get; init; }
    public sealed class Validator : AbstractValidator<AddAccountSignInInput>
    {
        public Validator(ILocalCulture culture)
        {
            RuleFor(x => x.Account).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.AccountIsRequired));
            RuleFor(x => x.Password).NotEmpty().WithMessage(culture.Link(HumanResourcesFlag.WrongPassword));
        }
    }
}