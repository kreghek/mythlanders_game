using System.Diagnostics.CodeAnalysis;

namespace Core.Dices;

/// <summary>
/// The implementation of the dice working by linear law.
/// </summary>
public class LinearDice : IDice
{
    private readonly Random _random;

    /// <summary>
    /// The constructor.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public LinearDice()
    {
        _random = new Random();
    }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="seed"> The randomization seed. </param>
    /// <remarks>
    /// Same seeds make same random number sequence.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public LinearDice(int seed)
    {
        _random = new Random(seed);
    }

    /// <inheritdoc />
    public int Roll(int n)
    {
        var rollResult = _random.Next(1, n + 1);
        return rollResult;
    }
}