namespace GameClient.Engine.MoveFunctions;

/// <summary>
/// Move function value.
/// </summary>
public record MoveFunctionArgument
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="argumentValue">base value</param>
    /// <exception cref="ArgumentOutOfRangeException">Raise the value less 0 or greater that 1.</exception>
    public MoveFunctionArgument(double argumentValue)
    {
        if (argumentValue is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(argumentValue));
        }

        Value = argumentValue;
    }

    /// <summary>
    /// Maximum function value.
    /// </summary>
    public static MoveFunctionArgument Max => new(1);

    /// <summary>
    /// Value.
    /// </summary>
    public double Value { get; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static implicit operator MoveFunctionArgument(double value) { return new MoveFunctionArgument(value); }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}