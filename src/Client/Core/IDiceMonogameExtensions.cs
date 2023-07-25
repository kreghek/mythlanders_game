using CombatDicesTeam.Dices;

using Microsoft.Xna.Framework;

namespace Client.Core;

internal static class IDiceMonogameExtensions
{
    public static Vector2 RollPoint(this IDice dice, Rectangle area)
    {
        var x = dice.Roll(area.Left, area.Right);
        var y = dice.Roll(area.Top, area.Bottom);

        return new Vector2(x, y);
    }
}