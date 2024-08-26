namespace Eywa.Serve.Modularity.Commons.Attacheds.Enumerates;
public enum AccountAccessFlag
{
    IncorrectUserRole,
    UserRoleNotEmpty,

    IncorrectAccessPermissions,
    IncorrectModulePermissions,

    TimeIntervalNotEmpty,
    TimeIntervalQueryFormatIncorrect,
    StartTimeFormatError,
    EndTimeFormatError,
    StartTimeMismatchEndTime,
}