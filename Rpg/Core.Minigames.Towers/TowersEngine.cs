namespace Core.Minigames.Towers;

public sealed class TowersEngine
{
    public TowersEngine(int[][] initBars)
    {
    }

    public void MoveRing(TowerBar sourceBar, TowerBar targetBar)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<TowerRingMovedEventArgs>? TowerRingMoved;

    public event EventHandler? Complete;

    public IReadOnlyList<TowerBar> Bars { get; }
}