namespace Core.Minigames.Towers;

public sealed class TowerBar
{
    public TowerBar(IReadOnlyList<TowerRing> rings, bool isVictoryTarget)
    {
        Rings = rings;
        IsVictoryTarget = isVictoryTarget;
    }

    public bool IsVictoryTarget { get; }

    public IReadOnlyList<TowerRing> Rings { get; }

    public TowerRing PullOf()
    {
        throw new NotImplementedException();
    }

    public void PutOn(TowerRing ring)
    {
        throw new NotImplementedException();
    }
}