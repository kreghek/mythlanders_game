using CombatDicesTeam.Dices;

using Core.Combats;

namespace Core.Minigames.Match3;

public interface IGemSource
{
    GemColor GetNextGem();
}

public sealed class GemSource : IGemSource {
    private readonly IDice _dice;

    public GemSource(IDice dice)
    {
        _dice = dice;
    }

    public GemColor GetNextGem()
    {
        return _dice.RollFromList(new[]
        {
            GemColor.Red,
            GemColor.Green,
            GemColor.Blue
        });
    }
}

public sealed class Match3Engine
{
    private readonly IGemSource _gemSource;

    public Match3Engine(Matrix<GemColor> initialField, IGemSource gemSource)
    {
        _gemSource = gemSource;
        Field = initialField;
    }

    public Matrix<GemColor> Field { get; }

    public void Swap(Coords c1, Coords c2)
    {
        // ReSharper disable once SwapViaDeconstruction
        var target = Field[c2.Column, c2.Row];
        Field[c2.Column, c2.Row] = Field[c1.Column, c1.Row];
        Field[c1.Column, c1.Row] = target;

        GemSwapped?.Invoke(this, new GemSwappedEventArgs(c1, c2));
    }

    public void PrepareField()
    {
        Down(Field.Items, Field.Width, Field.Height);
    }
    
    private static void Down(GemColor[,] matrix, int width, int height)
    {
        var current = Enumerable.Repeat(height - 1, width).ToArray();
        for(var row = height - 1; row >= 0; row--)
        {
            for(var col = 0; col < width; col++)
            {
                if (matrix[col, row] != GemColor.Empty)
                {
                    if (current[col] != row)
                    {
                        // ReSharper disable once SwapViaDeconstruction
                        var t = matrix[col, row];
                        matrix[col, row] = matrix[col, current[col]];
                        matrix[col, current[col]] = t;
						
                    }
                    current[col]--;
                }
            }
        }
    }

    public void Handle()
    {
        for (var col = 0; col < Field.Width; col++)
        {
            for (var row = 0; row < Field.Height; row++)
            {
                var current = Field[col, row];

                var horSum = MatchHorizontally(current, col, row);
                var verSum = MatchVertically(current, col, row);
            }
        }
    }

    private int? MatchHorizontally(GemColor matchingGem, int col, int row)
    {
        var rightCount = Match(matchingGem, col+1, row, 1, 0, 0);
        var leftCount = Match(matchingGem, col-1, row, -1, 0, 0);

        var sum = rightCount + leftCount + 1;
        if (sum >= 3)
        {
            var leftBorder = col - leftCount;
            var rightBorder = col + rightCount;

            for (var i = leftBorder; i <= rightBorder; i++)
            {
                GemMatched?.Invoke(this, new GemMatchedEventArgs(new Coords(i, row)));
                Field[leftBorder + i, row] = GemColor.Empty;
            }

            return sum;
        }

        return null;
    }

    private int? MatchVertically(GemColor matchingGem, int col, int row)
    {
        var bottomCount = Match(matchingGem, col, row + 1, 0, 1, 0);
        var topCount = Match(matchingGem, col, row - 1, 0, -1, 0);

        var sum = bottomCount + topCount + 1;
        if (sum >= 3)
        {
            var topBorder = col - topCount;
            var bottomBorder = col + bottomCount;

            for (var i = topBorder; i <= bottomBorder; i++)
            {
                GemMatched?.Invoke(this, new GemMatchedEventArgs(new Coords(i, row)));
                Field[col, topBorder + i] = GemColor.Empty;
            }

            return sum;
        }

        return null;
    }

    private int Match(GemColor matchingGem, int col, int row, int colDiff, int rowDiff, int currentCount)
    {
        if (col > Field.Width - 1 || row > Field.Height - 1 ||
            col < 0 || row < 0)
        {
            return currentCount;
        }

        if (Field[col, row] != matchingGem || Field[col, row] == GemColor.Empty)
        {
            return currentCount;
        }

        return Match(matchingGem, col + colDiff, row + rowDiff, colDiff, rowDiff, currentCount + 1);
    }

    public event EventHandler<GemSwappedEventArgs>? GemSwapped;
    public event EventHandler<GemMatchedEventArgs>? GemMatched;
}