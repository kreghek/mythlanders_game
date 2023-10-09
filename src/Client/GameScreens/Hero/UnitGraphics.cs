using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Hero;

internal sealed class UnitGraphics : UnitGraphicsBase
{
    public UnitGraphics(UnitName spriteSheetId, CombatantGraphicsConfigBase graphicsConfig, bool isNormalOrientation,
        Vector2 position, GameObjectContentStorage gameObjectContentStorage) :
        base(spriteSheetId, graphicsConfig, isNormalOrientation, position, gameObjectContentStorage)
    {
    }
}