using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal class GeneralInfoPanel: ControlBase
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

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
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
            
            for (var statIndex = 0; statIndex < sb.Count; statIndex++)
            {
                var line = sb[statIndex];
                spriteBatch.DrawString(_mainFont, line,
                    new Vector2(contentRect.Center.X, contentRect.Top + statIndex * 22), Color.White);
            }
        }
    }
    
    internal class PerkInfoPanel: ControlBase
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

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
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
                    new Vector2(contentRect.Center.X, contentRect.Top + statIndex * 22), Color.White);
            }
        }
    }
}