using System;

using Microsoft.Xna.Framework;

namespace Client.Assets.InteractionDeliveryObjects;

internal sealed class RotateForward : IRotationFunction
{
    private readonly Vector2 _startPosition;
    private readonly Vector2 _targetPosition;

    public RotateForward(Vector2 startPosition, Vector2 targetPosition)
    {
        _startPosition = startPosition;
        _targetPosition = targetPosition;
    }

    /// <inheritdoc />
    public float CalculateRadianAngle(float t)
    {
        return MathF.Atan2(_targetPosition.Y - _startPosition.Y, _targetPosition.X - _startPosition.X);
    }
}