namespace Eywa.Core.Architects.Primaries.Composers;
public abstract class BattleshipBuilder
{
    public abstract BattleshipBuilder Add();
    public abstract ValueTask<BattleshipBuilder> RunAsync();
    public async Task AsyncCallback() => await Add().RunAsync().ConfigureAwait(false);
}