namespace Eywa.Serve.Modularity.Commons.Attacheds.Enumerates;
public enum HumanResourcesFlag
{
    ModuleName,

    UserIdIsRequired,
    UserNoIsRequired,
    UserNoIndex,
    UserNameIsRequired,
    EmailIsRequired,
    EmailFormatError,
    EmailIndex,
    SignInFailed,
    TokenInvalid,
    UserIdDoesNotExist,

    AccountInvalid,
    AccountIsRequired,
    AccountDoesNotExist,

    PasswordIsRequired,
    WrongPassword,
    SystemAccountIsRequired,
    SystemAccountAlreadyExists,

    NameIdentifierIsRequired,
    RefreshTokenIsRequired,
    RolePolicyMismatch,
    AccessTypeMismatch,
    NoPermissionToCreateAdministrator,

    ModuleIdIsRequired,
    ModuleIdDoesNotExist,
    ModuleUserAlreadyExists,
    DomainTypeExisted,
    DomainTypeMismatch,

    PositionIdRequired,
    PositionIdDoesNotExist,
    PositionNoIsRequired,
    PositionNoIndex,
    PositionNameIsRequired,
}