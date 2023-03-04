using System;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class UnitButton : ButtonBase
    {
        private readonly UnitGameObject _gameObject;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public UnitButton(UnitGameObject gameObject, GameObjectContentStorage gameObjectContentStorage)
        {
            _gameObject = gameObject;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        protected override Point CalcTextureOffset()
        {
            return Point.Zero;
        }

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            //if (_buttonState == UiButtonState.Hover)
            //{
            //    base.DrawBackground(spriteBatch, new Color(200, 0, 0, 50));
            //}
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            //var unitMarkerTexture = _gameObjectContentStorage.GetCombatUnitMarker();
            //if (_buttonState == UiButtonState.Hover)
            //{
            //    spriteBatch.Draw(unitMarkerTexture,
            //        new Rectangle(Rect.X, Rect.Bottom - 32, 128, 32),
            //        new Rectangle(0, 32, 128, 32),
            //        Color.White);
            //}
            //else
            //{
            //    spriteBatch.Draw(unitMarkerTexture,
            //        new Rectangle(Rect.X + 8, Rect.Bottom - 32 + 8, 128 - 16, 32 - 8),
            //        new Rectangle(0, 32, 128, 32),
            //        new Color(Color.White, 0.25f));
            //}
        }

        protected override bool IsMouseOver(Rectangle mouseRect)
        {
            var center = Rect.Center.ToVector2();

            var distance = (center - mouseRect.Location.ToVector2()).Length();

            var radius = Math.Min(Rect.Width, Rect.Height) / 2;

            return distance <= radius;
        }
    }
}