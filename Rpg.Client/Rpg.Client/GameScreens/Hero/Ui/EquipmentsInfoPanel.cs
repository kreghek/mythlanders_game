using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Hero.Ui
{
    internal sealed class EquipmentsInfoPanel : PanelBase
    {
        private const int ICON_SIZE = 64;
        private readonly IList<EntityIconButton<Equipment>> _equipmentButtons;
        private readonly Unit _hero;
        private readonly SpriteFont _mainFont;
        private readonly Texture2D _hintTexture;
        private readonly Player _player;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private TextHint? _equipmentHint;
        private Equipment? _equipmentUnderHint;

        public EquipmentsInfoPanel(Texture2D texture, SpriteFont titleFont, Unit hero, SpriteFont mainFont,
            Texture2D controlTexture, Texture2D equipmentIconsTexture, Texture2D hintTexture, Player player, ResolutionIndependentRenderer resolutionIndependentRenderer) : base(
            texture, titleFont)
        {
            _hero = hero;
            _mainFont = mainFont;
            _hintTexture = hintTexture;
            _player = player;
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _equipmentButtons = new List<EntityIconButton<Equipment>>();
            for (var index = 0; index < _hero.Equipments.Count; index++)
            {
                var equipment = _hero.Equipments[index];
                var equipmentIconRect = GetEquipmentIconRect(equipment.Scheme.Sid);

                var equipmentIconButton = new EntityIconButton<Equipment>(controlTexture,
                    new IconData(equipmentIconsTexture, equipmentIconRect), equipment);
                _equipmentButtons.Add(equipmentIconButton);
                
                equipmentIconButton.OnClick += EquipmentIconButton_OnClick;
            }
        }

        private void EquipmentIconButton_OnClick(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                Debug.Fail("Sender must be assigned. Use handler only for instantiated classes, not on static ones.");
                return;
            }

            var equipment = ((EntityIconButton<Equipment>)sender).Entity;
            var resourceItem = _player.Inventory.Single(x => x.Type == equipment.Scheme.RequiredResourceToLevelUp);
            
            
            resourceItem.Amount -= equipment.RequiredResourceAmountToLevelUp;
            equipment.LevelUp();
            ClearEquipmentHint();
            CreateHint(equipment);
        }

        protected override string TitleResourceId => nameof(UiResource.HeroEquipmentInfoTitle);

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleEquipmentHint();

            foreach (var equipmentButton in _equipmentButtons)
            {
                var equipment = equipmentButton.Entity;
                var resourceItem = _player.Inventory.Single(x => x.Type == equipment.Scheme.RequiredResourceToLevelUp);
                var equipmentResourceAmount = resourceItem.Amount;
                if (equipmentResourceAmount >= equipment.RequiredResourceAmountToLevelUp)
                {
                    equipmentButton.IsEnabled = true;
                }
                else
                {
                    equipmentButton.IsEnabled = false;
                }
                
                equipmentButton.Update(_resolutionIndependentRenderer);
            }
        }

        private void HandleEquipmentHint()
        {
            var mouse = Mouse.GetState();
            var mouseRect = new Rectangle(mouse.Position, new Point(1, 1));
            Equipment? currentEquipment = null;
            foreach (var equipmentButton in _equipmentButtons)
            {
                if (equipmentButton.Rect.Contains(mouseRect))
                {
                    currentEquipment = equipmentButton.Entity;

                    if (_equipmentUnderHint is null)
                    {
                        CreateHint(equipmentButton.Entity);
                    }

                    if (_equipmentUnderHint != equipmentButton.Entity)
                    {
                        CreateHint(equipmentButton.Entity);
                    }
                }
            }

            if (currentEquipment is null)
            {
                ClearEquipmentHint();
            }

            if (_equipmentHint is not null)
            {
                _equipmentHint.Rect = new Rectangle(mouse.Position, new Point(100, 50));
            }
        }

        private void ClearEquipmentHint()
        {
            _equipmentUnderHint = null;
            _equipmentHint = null;
        }

        private void CreateHint(Equipment equipment)
        {
            _equipmentUnderHint = equipment;
            
            var entityNameText = GameObjectHelper.GetLocalized(equipment.Scheme.Sid);
            var entityInfoText = $"{entityNameText} ({equipment.Level} lvl)";
            var upgradeInfoText =
                $"{equipment.Scheme.RequiredResourceToLevelUp}x{equipment.RequiredResourceAmountToLevelUp} to levelup";
            
            _equipmentHint = new TextHint(_hintTexture, _mainFont, $"{entityInfoText}{Environment.NewLine}{upgradeInfoText}");
        }

        protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            for (var index = 0; index < _equipmentButtons.Count; index++)
            {
                var equipmentButton = _equipmentButtons[index];
                const int MARGIN = 5;
                equipmentButton.Rect = new Rectangle(
                    contentRect.Location + new Point(MARGIN, MARGIN + index * (ICON_SIZE + MARGIN)),
                    new Point(ICON_SIZE, ICON_SIZE));
                equipmentButton.Draw(spriteBatch);

                var equipment = equipmentButton.Entity;
                var entityNameText = GameObjectHelper.GetLocalized(equipment.Scheme.Sid);
                var entityInfoText = $"{entityNameText} ({equipment.Level} lvl)";
                spriteBatch.DrawString(_mainFont, entityInfoText,
                    equipmentButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 0), Color.Wheat);

                var upgradeInfoText =
                    $"{equipment.Scheme.RequiredResourceToLevelUp}x{equipment.RequiredResourceAmountToLevelUp} to levelup";
                spriteBatch.DrawString(_mainFont, upgradeInfoText,
                    equipmentButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 20), Color.Wheat);
            }

            _equipmentHint?.Draw(spriteBatch);
        }

        private static int? GetEquipmentIconOneBasedIndex(EquipmentSid schemeSid)
        {
            return schemeSid switch
            {
                EquipmentSid.CombatSword => 1,
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