namespace Core.Minigames.Match3;

public sealed class GemSwappedEventArgs: EventArgs
{
    public GemSwappedEventArgs(Coords c1, Coords c2)
    {
        C1 = c1;
        C2 = c2;
    }

    public Coords C1 { get; }
    public Coords C2 { get; }
}

public sealed class GemMatchedEventArgs: EventArgs
{
    public Coords MatchedCoords { get; }

    public GemMatchedEventArgs(Coords coords)
    {
        MatchedCoords = coords;
    }
}