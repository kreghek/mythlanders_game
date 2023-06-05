using Microsoft.Xna.Framework;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal interface IUnitPositionProvider
    {
        Vector2 GetPosition(int slotIndex, bool isPlayerSide);
    }
}