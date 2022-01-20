using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.CharacterDetails.Ui
{
    internal class PerkInfoPanel : PanelBase
    {
        private readonly Unit _character;
        private readonly SpriteFont _mainFont;

        public PerkInfoPanel(Texture2D texture, SpriteFont titleFont, Unit character, SpriteFont mainFont) : base(
            texture, titleFont)
        {
            _character = character;
            _mainFont = mainFont;
        }

        protected override string TitleResourceId => nameof(UiResource.CharacterPerkInfoTitle);

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var sb = new List<string>();

            foreach (var perk in _character.Perks)
            {
                var localizedName = GameObjectHelper.GetLocalized(perk);
                sb.Add(localizedName);

                var localizedDescription = GameObjectHelper.GetLocalizedDescription(perk);
                sb.Add(localizedDescription);
            }

            for (var statIndex = 0; statIndex < sb.Count; statIndex++)
            {
                var line = sb[statIndex];
                spriteBatch.DrawString(_mainFont, line,
                    new Vector2(contentRect.Left, contentRect.Top + statIndex * 22), Color.SaddleBrown);
            }
        }
    }
}