using Core.Combats;

using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class UnitPositionProvider : IUnitPositionProvider
{
    private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly Matrix<Vector2> _unitPredefinedPositions;

    public UnitPositionProvider(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _unitPredefinedPositions = new Matrix<Vector2>(2, 3)
        {
            [0, 0] = new Vector2(305, 250),
            [0, 1] = new Vector2(335, 300),
            [0, 2] = new Vector2(305, 350),
            [1, 0] = new Vector2(215, 250),
            [1, 1] = new Vector2(245, 300),
            [1, 2] = new Vector2(215, 350),
        };
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
    }

    public Vector2 GetPosition(FieldCoords formationCoords, bool isPlayerSide)
    {
        var predefinedPosition = _unitPredefinedPositions[formationCoords.ColumentIndex, formationCoords.LineIndex];

        Vector2 calculatedPosition;

        if (isPlayerSide)
        {
            calculatedPosition = predefinedPosition;
        }
        else
        {
            var width = _resolutionIndependentRenderer.VirtualWidth;
            // Move from right edge.
            var xMirror = width - predefinedPosition.X;
            calculatedPosition = new Vector2(xMirror, predefinedPosition.Y);
        }

        return calculatedPosition;
    }
}