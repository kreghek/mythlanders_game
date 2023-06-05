using Microsoft.Xna.Framework;

namespace Rpg.Client.Core
{
    internal interface IBattlefieldInteractionContext
    {
        public Rectangle GetArea(Team side);
    }
}