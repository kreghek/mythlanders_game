using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.CharacterDetails;

namespace Rpg.Client.GameScreens.Hero.Ui
{
    internal sealed class SkillsInfoPanel : PanelBase
    {
        private readonly Unit _character;
        private readonly SpriteFont _mainFont;

        public SkillsInfoPanel(Texture2D texture, SpriteFont titleFont, Unit character, SpriteFont mainFont) : base(
            texture, titleFont)
        {
            _character = character;
            _mainFont = mainFont;
        }

        protected override string TitleResourceId => nameof(UiResource.HeroSkillsInfoTitle);

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var sb = new List<string>();

            foreach (var skill in _character.Skills)
            {
                var skillNameText = GameObjectHelper.GetLocalized(skill.Sid);

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
                    new Vector2(contentRect.Left, contentRect.Top + statIndex * 22), Color.Wheat);
            }
        }
    }
    
    internal sealed class EquipmentsInfoPanel : PanelBase
    {
        private readonly Unit _hero;
        private readonly SpriteFont _mainFont;
        private readonly Texture2D _equipmentIconsTexture;
        private readonly IList<EntityIconButton<Equipment>> _equipmentIcons;

        public EquipmentsInfoPanel(Texture2D texture, SpriteFont titleFont, Unit hero, SpriteFont mainFont, Texture2D controlTexture, Texture2D equipmentIconsTexture) : base(
            texture, titleFont)
        {
            _hero = hero;
            _mainFont = mainFont;
            _equipmentIconsTexture = equipmentIconsTexture;
            _equipmentIcons = new List<EntityIconButton<Equipment>>();
            for (var index = 0; index < _hero.Equipments.Count; index++)
            {
                var equipment = _hero.Equipments[index];
                var equipmentIconRect = GetEquipmentIconRect(equipment.Scheme.Sid);

                var equipmentIconButton = new EntityIconButton<Equipment>(controlTexture,
                    new IconData(equipmentIconsTexture, equipmentIconRect), equipment);
                _equipmentIcons.Add(equipmentIconButton);
                
                var skillNameText = GameObjectHelper.GetLocalized(equipment.Scheme.Sid);
            }
        }

        protected override string TitleResourceId => nameof(UiResource.HeroEquipmentInfoTitle);

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            for (var index = 0; index < _equipmentIcons.Count; index++)
            {
                var equipmentButton = _equipmentIcons[index];
                equipmentButton.Rect = new Rectangle(contentRect.Location + new Point(index * (64 + 5), 0),
                    new Point(64, 64));
                equipmentButton.Draw(spriteBatch);
            }
        }

        private static Rectangle GetEquipmentIconRect(EquipmentSid schemeSid)
        {
            return new Rectangle(0, 0, 64, 64);
        }
    }
}