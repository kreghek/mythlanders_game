namespace Core.Minigames.Towers;

public sealed class TowerBar
{
    public IReadOnlyList<TowerRing> Rings { get; }

    public TowerRing PullOf()
    {
        throw new NotImplementedException();
    }

    public void PutOn(TowerRing ring)
    {
        throw new NotImplementedException();
    }

    public bool IsVictoryTarget { get; }
}

public sealed class TowerRing
{
    public int Size { get; }
}