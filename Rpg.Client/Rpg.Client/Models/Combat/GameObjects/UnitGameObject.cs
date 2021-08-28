using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitGameObject
    {
        private readonly Sprite _graphics;
        private readonly Sprite _selectedMarker;

        public UnitGameObject(CombatUnit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            _graphics = new Sprite(gameObjectContentStorage.GetUnitGraphics()) {
                Position = position,
                Origin = new Vector2(0.5f, 0.75f),
                FlipX = !unit.Unit.IsPlayerControlled
            };

            _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
            {
                Position = position,
                Origin = new Vector2(0.5f, 0.75f)
            };

            Unit = unit;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphics.Draw(spriteBatch);

            if (IsActive)
            {
                _selectedMarker.Draw(spriteBatch);
            }
        }

        public bool IsActive { get; set; }
        public CombatUnit? Unit { get; internal set; }
    }
}
