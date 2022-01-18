using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Upgrade.Ui
{
    internal sealed class SelectCharacterEventArgs : EventArgs
    {
        public SelectCharacterEventArgs(Unit character)
        {
            Character = character;
        }

        public Unit Character { get; }
    }

    internal sealed class CharacterPanel : ControlBase
    {
        private readonly ButtonBase _infoButton;
        private readonly Unit _character;
        private readonly Texture2D _portraitTexture;
        private readonly SpriteFont _nameFont;
        private readonly SpriteFont _mainFont;

        public CharacterPanel(Texture2D texture, Unit character, Texture2D buttonTexture, SpriteFont buttonFont, Texture2D portraitTexture, SpriteFont nameFont, SpriteFont mainFont) : base(texture)
        {
            _character = character;
            _portraitTexture = portraitTexture;
            _nameFont = nameFont;
            _mainFont = mainFont;

            _infoButton = new IndicatorTextButton(nameof(UiResource.InfoButtonTitle), buttonTexture, buttonFont);
            _infoButton.OnClick += (_, _) =>
            {
                SelectCharacter?.Invoke(this, new SelectCharacterEventArgs(character));
            };
        }

        public event EventHandler<SelectCharacterEventArgs> SelectCharacter;

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            DrawPortrait(spriteBatch, contentRect);
            DrawName(spriteBatch, contentRect);
            DrawInfoButton(spriteBatch, contentRect);

            spriteBatch.DrawString(_mainFont, string.Format(UiResource.CombatLevelTemplate, _character.Level), contentRect.Location.ToVector2() + new Vector2(32 + 5, 10), Color.Black);
        }

        private void DrawInfoButton(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            _infoButton.Rect = new Rectangle(contentRect.Left + 32, contentRect.Top + 32, 100, 20);
            _infoButton.Draw(spriteBatch);
        }

        private void DrawName(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var unitName = _character.UnitScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);
            spriteBatch.DrawString(_nameFont, name, contentRect.Location.ToVector2() + new Vector2(32 + 5, 0), Color.Black);
        }

        private void DrawPortrait(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var portraitRect = UnsortedHelpers.GetUnitPortraitRect(_character.UnitScheme.Name);
            spriteBatch.Draw(_portraitTexture, new Rectangle(contentRect.Location, new Point(32, 32)), portraitRect, Color.White);
        }

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _infoButton.Update(resolutionIndependentRenderer);
        }
    }
}