namespace Eywa.Serve.Modularity.Commons.Structures.Quarterlies;
public interface ILocalCulture
{
    string Link(in RolePolicy type);
    string Link(in DomainType type);
    string Link(in LoginStatus type);
    string Link(in AccountAccessFlag type);
    string Link(in HumanResourcesFlag type);
    string Link(in ProductionControlFlag type);
    //(DateTime startTime, DateTime endTime) FormatToTimeInterval(in string? input, in bool notNull = false);
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class LocalCulture : ILocalCulture
{
    public string Link(in RolePolicy type) => Localizer[type.ToString()];
    public string Link(in DomainType type) => Localizer[type.ToString()];
    public string Link(in LoginStatus type) => Localizer[type.ToString()];
    public string Link(in AccountAccessFlag type) => Localizer[type.ToString()];
    public string Link(in HumanResourcesFlag type) => Localizer[type.ToString()];
    public string Link(in ProductionControlFlag type) => Localizer[type.ToString()];
    //public (DateTime startTime, DateTime endTime) FormatToTimeInterval(in string? input, in bool notNull = false)
    //{
    //    if (input is null) return notNull ?
    //            throw new Exception(Link(InfraUniversalizerFlag.TimeIntervalNotEmpty)) : (default, default);
    //    var intervals = input.TrimText('@');
    //    if (intervals.Length != 2) throw new Exception(
    //        $"{Link(InfraUniversalizerFlag.TimeIntervalQueryFormatIncorrect)} [{input}]");
    //    var startTime = intervals[0].ParseDateTime();
    //    if (startTime == default) throw new Exception(
    //        $"{Link(InfraUniversalizerFlag.StartTimeFormatError)} [{input}]");
    //    var endTime = intervals[1].ParseDateTime();
    //    if (endTime == default) throw new Exception(
    //        $"{Link(InfraUniversalizerFlag.EndTimeFormatError)} [{input}]");
    //    return startTime >= endTime ? throw new Exception(
    //        $"{Link(InfraUniversalizerFlag.StartTimeMismatchEndTime)} [{input}]") : (startTime, endTime);
    //}
    public required IStringLocalizer<LocalCulture> Localizer { get; init; }
}