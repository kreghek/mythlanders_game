using Client.Core;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class BattlefieldInteractionContext : IBattlefieldInteractionContext
{
    public Rectangle GetArea(Team side)
    {
        if (side == Team.Cpu)
        {
            return new Rectangle(new Point(100 + 400, 100), new Point(200, 200));
        }

        return new Rectangle(new Point(100, 100), new Point(200, 200));
    }
}