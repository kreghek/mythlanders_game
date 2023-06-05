using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Hero
{
    internal sealed class UnitGraphics : UnitGraphicsBase
    {
        public UnitGraphics(Unit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage) : base(unit,
            position, gameObjectContentStorage)
        {
        }
    }
}