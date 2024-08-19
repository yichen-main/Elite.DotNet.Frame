namespace Elite.Core.Architects.Primaries.Composers;
internal abstract class BattleshipBuilder
{
    public abstract ValueTask<BattleshipBuilder> AddAsync();
    public abstract ValueTask<BattleshipBuilder> RunAsync();
    public async Task AsyncCallback()
    {
        await AddAsync().ConfigureAwait(false);
        await RunAsync().ConfigureAwait(false);
    }
}