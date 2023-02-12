using Core.Combats;

namespace Core.Minigames.Match3;

public sealed class Match3Engine
{
    public Match3Engine(Matrix<GemColor> initialField)
    {
        Field = initialField;
    }

    public Matrix<GemColor> Field { get; }

    public void Swap(Coords c1, Coords c2)
    {
        throw new NotImplementedException();
    }


    public event EventHandler<GemSwappedEveentArgs>? GemSwapped;
}