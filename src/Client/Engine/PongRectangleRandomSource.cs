using CombatDicesTeam.Dices;

using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;

namespace Client.Engine;

internal sealed class PongRectangleRandomSource : IPongRectangleRandomSource
{
    private readonly IDice _dice;
    private readonly float _vectorLength;

    public PongRectangleRandomSource(IDice dice, float vectorLength)
    {
        _dice = dice;
        _vectorLength = vectorLength;
    }

    public Vector2 GetRandomVector()
    {
        var vector = new Vector2(_dice.RollD100() - 50, _dice.RollD100() - 50);
        vector.Normalize();

        return vector * _vectorLength;
    }
}