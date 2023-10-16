﻿namespace GameClient.Engine.MoveFunctions;

/// <summary>
/// Move function value.
/// </summary>
public record MoveFunctionValue
{
    private readonly double _value;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="value">base value</param>
    /// <exception cref="ArgumentOutOfRangeException">Raise the value less 0 or greater that 1.</exception>
    public MoveFunctionValue(double value)
    {
        if (value < 0 || value > 1)
        { 
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        _value = value;
    }

    /// <summary>
    /// Value.
    /// </summary>
    public double Value => _value;

    /// <summary>
    /// Maximim function value.
    /// </summary>
    public static MoveFunctionValue Max => new MoveFunctionValue(1);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static implicit operator MoveFunctionValue(double value) {  return new MoveFunctionValue(value); }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}