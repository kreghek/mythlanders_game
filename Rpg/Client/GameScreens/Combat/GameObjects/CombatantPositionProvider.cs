using Core.Combats;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class CombatantPositionProvider : ICombatantPositionProvider
{
    private readonly int _combatFieldWidth;
    private readonly Matrix<Vector2> _predefinedPositions;

    public CombatantPositionProvider(int combatFieldWidth)
    {
        _predefinedPositions = new Matrix<Vector2>(2, 3)
        {
            [0, 0] = new Vector2(309, 250),
            [0, 1] = new Vector2(335, 300),
            [0, 2] = new Vector2(300, 355),
            [1, 0] = new Vector2(210, 250),
            [1, 1] = new Vector2(245, 300),
            [1, 2] = new Vector2(219, 355)
        };
        _combatFieldWidth = combatFieldWidth;
    }

    public Vector2 GetPosition(FieldCoords formationCoords, CombatantPositionSide side)
    {
        var predefinedPosition = _predefinedPositions[formationCoords.ColumentIndex, formationCoords.LineIndex];

        Vector2 calculatedPosition;

        if (side == CombatantPositionSide.Heroes)
        {
            calculatedPosition = predefinedPosition;
        }
        else
        {
            // Move from right edge.
            var xMirror = _combatFieldWidth - predefinedPosition.X;
            calculatedPosition = new Vector2(xMirror, predefinedPosition.Y);
        }

        return calculatedPosition;
    }
}