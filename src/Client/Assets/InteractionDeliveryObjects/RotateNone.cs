namespace Client.Assets.InteractionDeliveryObjects;

internal sealed class RotateNone : IRotationFunction
{
    private readonly float _predefinedAngle;

    public RotateNone(float predefinedAngle = 0)
    {
        _predefinedAngle = predefinedAngle;
    }

    /// <inheritdoc />
    public float CalculateRadianAngle(float t)
    {
        return _predefinedAngle;
    }
}