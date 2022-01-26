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
        private readonly Unit _hero;
        private readonly IList<EntityIconButton<Equipment>> _equipmentIcons;

        public EquipmentsInfoPanel(Texture2D texture, SpriteFont titleFont, Unit hero, SpriteFont mainFont, Texture2D controlTexture, Texture2D equipmentIconsTexture) : base(
            texture, titleFont)
        {
            _hero = hero;
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
                equipmentButton.Rect = new Rectangle(contentRect.Location + new Point(index * (ICON_SIZE + 5), 0),
                    new Point(ICON_SIZE, ICON_SIZE));
                equipmentButton.Draw(spriteBatch);
            }
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
                _ => null,
            };
        }
    }
}