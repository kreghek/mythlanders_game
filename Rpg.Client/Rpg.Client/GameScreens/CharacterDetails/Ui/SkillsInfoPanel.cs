using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal class SkillsInfoPanel: PanelBase
    {
        private readonly Unit _character;
        private readonly SpriteFont _mainFont;

        public SkillsInfoPanel(Texture2D texture, Unit character, SpriteFont mainFont) : base(texture)
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
            
            foreach (var skill in _character.Skills)
            {
                var skillNameText = GameObjectResources.ResourceManager.GetString(skill.Sid.ToString()) ??
                                    $"#Resource-{skill.Sid}";

                sb.Add(skillNameText);
                if (skill.ManaCost is not null)
                {
                    sb.Add(string.Format(UiResource.ManaCostLabelTemplate, skill.ManaCost));
                }

                // TODO Display skill efficient - damages, durations, etc.
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