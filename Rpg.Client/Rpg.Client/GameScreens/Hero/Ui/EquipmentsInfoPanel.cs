using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.CharacterDetails;

namespace Rpg.Client.GameScreens.Hero.Ui
{
    internal sealed class EquipmentsInfoPanel : PanelBase
    {
        private const int ICON_SIZE = 64;
        private readonly IList<EntityIconButton<Equipment>> _equipmentIcons;
        private readonly Unit _hero;
        private readonly SpriteFont _mainFont;

        public EquipmentsInfoPanel(Texture2D texture, SpriteFont titleFont, Unit hero, SpriteFont mainFont,
            Texture2D controlTexture, Texture2D equipmentIconsTexture) : base(
            texture, titleFont)
        {
            _hero = hero;
            _mainFont = mainFont;
            _equipmentIcons = new List<EntityIconButton<Equipment>>();
            for (var index = 0; index < _hero.Equipments.Count; index++)
            {
                var equipment = _hero.Equipments[index];
                var equipmentIconRect = GetEquipmentIconRect(equipment.Scheme.Sid);

                var equipmentIconButton = new EntityIconButton<Equipment>(controlTexture,
                    new IconData(equipmentIconsTexture, equipmentIconRect), equipment);
                _equipmentIcons.Add(equipmentIconButton);
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
                const int MARGIN = 5;
                equipmentButton.Rect = new Rectangle(contentRect.Location + new Point(MARGIN, MARGIN + index * (ICON_SIZE + MARGIN)),
                    new Point(ICON_SIZE, ICON_SIZE));
                equipmentButton.Draw(spriteBatch);

                var equipment = equipmentButton.Entity;
                var entityNameText = GameObjectHelper.GetLocalized(equipment.Scheme.Sid);
                var entityInfoText = $"{entityNameText} ({equipment.Level} lvl)";
                spriteBatch.DrawString(_mainFont, entityInfoText, equipmentButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 0), Color.Wheat);

                var upgradeInfoText =
                    $"{equipment.Scheme.RequiredResourceToLevelUp}x{equipment.RequiredResourceAmountToLevelUp} to levelup";
                spriteBatch.DrawString(_mainFont, upgradeInfoText, equipmentButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 20), Color.Wheat);
            }
        }

        private static int? GetEquipmentIconOneBasedIndex(EquipmentSid schemeSid)
        {
            return schemeSid switch
            {
                EquipmentSid.WarriorGreatSword => 1,
                EquipmentSid.Mk2MediumPowerArmor => 2,
                EquipmentSid.WoodenHandSculpture => 3,
                EquipmentSid.ArcherPulsarBow => 4,
                EquipmentSid.ArcherMk3ScoutPowerArmor => 5,
                EquipmentSid.SilverWindNecklace => 6,
                EquipmentSid.HerbBag => 7,
                EquipmentSid.WomanShort => 8,
                EquipmentSid.BookOfHerbs => 9,
                _ => null
            };
        }

        private static Rectangle GetEquipmentIconRect(EquipmentSid schemeSid)
        {
            var index = GetEquipmentIconOneBasedIndex(schemeSid);
            if (index is null)
            {
                return new Rectangle(0, 0, ICON_SIZE, ICON_SIZE);
            }

            var zeroBasedIndex = index.Value - 1;
            const int COL_COUNT = 3;
            var col = zeroBasedIndex % COL_COUNT;
            var row = zeroBasedIndex / COL_COUNT;
            return new Rectangle(col * ICON_SIZE, row * ICON_SIZE, ICON_SIZE, ICON_SIZE);
        }
    }
}