using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal class PerkInfoPanel: PanelBase
    {
        private readonly Unit _character;
        private readonly SpriteFont _mainFont;

        public PerkInfoPanel(Texture2D texture, Unit character, SpriteFont mainFont) : base(texture)
        {
            _character = character;
            _mainFont = mainFont;
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var sb = new List<string>();
            
            foreach (var perk in _character.Perks)
            {
                var localizedName = GameObjectResources.ResourceManager.GetString(perk.GetType().Name);
                sb.Add(localizedName ?? $"[{perk.GetType().Name}]");

                var localizedDescription =
                    GameObjectResources.ResourceManager.GetString($"{perk.GetType().Name}Description");
                if (localizedDescription is not null)
                {
                    sb.Add(localizedDescription);
                }
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