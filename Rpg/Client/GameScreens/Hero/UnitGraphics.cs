using Client.Core;
using Client.GameScreens;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Hero
{
    internal sealed class UnitGraphics : UnitGraphicsBase
    {
        public UnitGraphics(UnitName spriteSheetId, UnitGraphicsConfigBase graphicsConfig, bool isNormalOrientation,
            Vector2 position, GameObjectContentStorage gameObjectContentStorage) :
            base(spriteSheetId, graphicsConfig, isNormalOrientation, position, gameObjectContentStorage)
        {
        }
    }
}