namespace Core.Minigames.Towers;

public sealed class TowersEngine
{
    private readonly IList<TowerBar> _bars;

    public TowersEngine(int[][] initBars)
    {
        _bars = new List<TowerBar>();

        for (var i = 0; i < initBars.Length; i++)
        {
            var rings = initBars[i].Select(x => new TowerRing(x)).ToArray();

            var isVictoryTarget = i == initBars.Length - 1;
            _bars.Add(new TowerBar(rings, isVictoryTarget));
        }
    }

    public IReadOnlyList<TowerBar> Bars => _bars.ToArray();

    public void MoveRing(TowerBar sourceBar, TowerBar targetBar)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<TowerRingMovedEventArgs>? TowerRingMoved;

    public event EventHandler? Complete;
}