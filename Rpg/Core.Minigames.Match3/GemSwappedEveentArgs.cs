namespace Core.Minigames.Match3;

public sealed class GemSwappedEveentArgs: EventArgs
{
    public GemSwappedEveentArgs(Coords c1, Coords c2)
    {
        C1 = c1;
        C2 = c2;
    }

    public Coords C1 { get; }
    public Coords C2 { get; }
}