using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class UnitGraphics : UnitGraphicsBase
    {
        public UnitGraphics(Unit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage) : base(unit,
            position, gameObjectContentStorage)
        {
        }

        public bool IsDamaged { get; set; }

        public void ChangePosition(Vector2 position, Unit unit)
        {
            _position = position;
            InitializeSprites(unit.UnitScheme, unit.IsPlayerControlled);
        }
    }
}