using Microsoft.Xna.Framework;

namespace Client.Core;

internal interface IBattlefieldInteractionContext
{
    public Rectangle GetArea(Team side);
}