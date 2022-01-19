using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal class GeneralInfoPanel: PanelBase
    {
        private readonly Unit _character;
        private readonly SpriteFont _mainFont;

        public GeneralInfoPanel(Texture2D texture, Unit character, SpriteFont mainFont) : base(texture)
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
            var unitName = _character.UnitScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);

            var sb = new List<string>
            {
                name,
                string.Format(UiResource.HitPointsLabelTemplate, _character.MaxHitPoints),
                string.Format(UiResource.ManaLabelTemplate, _character.ManaPool,
                    _character.ManaPoolSize),
                string.Format(UiResource.CombatLevelTemplate, _character.Level),
                string.Format(UiResource.CombatLevelUpTemplate, _character.LevelUpXpAmount)
            };

            foreach (var equipment in _character.Equipments)
            {
                sb.Add($"{equipment.Scheme.Sid} ({equipment.Level} lvl)");
                sb.Add($"{equipment.Scheme.RequiredResourceToLevelUp}x{equipment.RequiredResourceAmountToLevelUp} to levelup");
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