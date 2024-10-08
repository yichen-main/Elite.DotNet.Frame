﻿namespace Eywa.Serve.Constructs.Grindstones.Composers;
public abstract class HostedService : BackgroundService
{
    const int initialSeconds = 1;
    protected enum Interval
    {
        Instant = 1,
        Moment = 10,
        Hour = 3600,
        Daily = 86400,
    }
    protected async Task ExecuteAsync<T>(T content, Func<ValueTask> task) where T : HostedService
    {
        try
        {
            await task().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await FileLayout.OutputAsync(new()
            {
                Type = content.GetType(),
                Line = e.GetLine(),
                Name = nameof(ExecuteAsync),
                Message = e.Message,
            }).ConfigureAwait(false);
        }
    }
    protected async Task KeepAsync<T>(T content, Func<ValueTask> task, Action<Exception>? e = default, bool initialSkip = default) where T : HostedService
    {
        var type = content.GetType();
        if (!Chargers.Any(x => x.Key.IsMatch(type.Name))) Chargers[type.Name] = initialSeconds;
        PeriodicTimer initialTimer = new(TimeSpan.FromTicks(TimeSpan.TicksPerSecond * Interval.Instant.GetHashCode()));
        while (await initialTimer.WaitForNextTickAsync().ConfigureAwait(false))
        {
            try
            {
                var seconds = initialSeconds;
                if (!ExistArchway(type.Name))
                {
                    if (initialSkip) seconds = Chargers[type.Name];
                }
                else seconds = Chargers[type.Name];
                PeriodicTimer timer = new(TimeSpan.FromTicks(TimeSpan.TicksPerSecond * seconds));
                while (await timer.WaitForNextTickAsync().ConfigureAwait(false))
                {
                    try
                    {
                        await task().ConfigureAwait(false);
                        if (seconds != Chargers[type.Name]) timer.Dispose();
                        Histories.TryRemove(type.Name, out _);
                    }
                    catch (Exception exception)
                    {
                        if (e is not null) e(exception);
                        if (Histories.TryGetValue(type.Name, out var value))
                        {
                            if (!value.IsMatch(exception.Message)) await FileLayout.OutputAsync(new()
                            {
                                Type = type,
                                Line = exception.GetLine(),
                                Name = nameof(Task),
                                Message = exception.Message,
                            }).ConfigureAwait(false);
                        }
                        else await FileLayout.OutputAsync(new()
                        {
                            Type = type,
                            Line = exception.GetLine(),
                            Name = nameof(Task),
                            Message = exception.Message,
                        }).ConfigureAwait(false);
                        Histories[type.Name] = exception.Message;
                    }
                }
            }
            catch (Exception exception)
            {
                if (e is not null) e(exception);
                await FileLayout.OutputAsync(new()
                {
                    Type = GetType(),
                    Line = exception.GetLine(),
                    Name = nameof(KeepAsync),
                    Message = exception.Message,
                }).ConfigureAwait(false);
            }
        }
        static bool ExistArchway(in string flag)
        {
            try
            {
                ReaderWriterLock.EnterReadLock();
                return IdentifyFlags.Contains(flag);
            }
            finally
            {
                ReaderWriterLock.ExitReadLock();
                Refresh(flag);
            }
            static void Refresh(in string flag)
            {
                try
                {
                    ReaderWriterLock.EnterWriteLock();
                    IdentifyFlags.Add(flag);
                }
                finally
                {
                    ReaderWriterLock.ExitWriteLock();
                }
            }
        }
    }
    protected static void SetInterval<T>(in T content, in Interval type) where T : notnull => Chargers[content.GetType().Name] = type.GetHashCode();
    static ConcurrentDictionary<string, string> Histories { get; } = [];
    static ConcurrentDictionary<string, int> Chargers { get; } = [];
    static ReaderWriterLockSlim ReaderWriterLock { get; } = new();
    static HashSet<string> IdentifyFlags { get; } = [];
}