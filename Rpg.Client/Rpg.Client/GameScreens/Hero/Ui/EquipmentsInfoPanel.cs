using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Assets.Equipments;
using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Hero.Ui
{
    internal sealed class EquipmentsInfoPanel : PanelBase
    {
        private const int ICON_SIZE = 64;
        private const int MARGIN = 5;

        private readonly IList<EntityIconButton<Equipment>> _equipmentButtons;
        private readonly Texture2D _hintTexture;
        private readonly SpriteFont _mainFont;
        private readonly Player _player;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private TextHint? _equipmentHint;
        private Equipment? _equipmentUnderHint;
        private EntityIconButton<Equipment> _equipmntButtonUnderHint;

        public EquipmentsInfoPanel(Texture2D texture, SpriteFont titleFont, Unit hero, SpriteFont mainFont,
            Texture2D controlTexture, Texture2D equipmentIconsTexture, Texture2D hintTexture, Player player,
            ResolutionIndependentRenderer resolutionIndependentRenderer) : base(
            texture, titleFont)
        {
            _mainFont = mainFont;
            _hintTexture = hintTexture;
            _player = player;
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _equipmentButtons = new List<EntityIconButton<Equipment>>();
            for (var index = 0; index < hero.Equipments.Count; index++)
            {
                var equipment = hero.Equipments[index];
                var equipmentIconRect = GetEquipmentIconRect(equipment.Scheme.Metadata);

                var equipmentIconButton = new EntityIconButton<Equipment>(controlTexture,
                    new IconData(equipmentIconsTexture, equipmentIconRect), equipment);
                _equipmentButtons.Add(equipmentIconButton);

                equipmentIconButton.OnClick += EquipmentIconButton_OnClick;
            }
        }

        protected override string TitleResourceId => nameof(UiResource.HeroEquipmentInfoTitle);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleEquipmentHint();

            foreach (var equipmentButton in _equipmentButtons)
            {
                var equipment = equipmentButton.Entity;

                equipmentButton.IsEnabled = CheckUpgradeIsAvailable(equipment);

                equipmentButton.Update(_resolutionIndependentRenderer);
            }
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            for (var index = 0; index < _equipmentButtons.Count; index++)
            {
                var equipmentButton = _equipmentButtons[index];

                equipmentButton.Rect = new Rectangle(
                    contentRect.Location + new Point(MARGIN, MARGIN + index * (ICON_SIZE + MARGIN)),
                    new Point(ICON_SIZE, ICON_SIZE));
                equipmentButton.Draw(spriteBatch);

                var equipment = equipmentButton.Entity;
                var entityNameText = GameObjectHelper.GetLocalized(equipment.Scheme.Sid);
                var entityInfoText = string.Format(UiResource.EquipmentTitleTemplate, entityNameText, equipment.Level);
                spriteBatch.DrawString(_mainFont, entityInfoText,
                    equipmentButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 0), Color.Wheat);

                var upgradeIsAvailable = CheckUpgradeIsAvailable(equipment);
                if (upgradeIsAvailable)
                {
                    var upgradeInfoText = UiResource.EquipmentUpgradeMarkerText;
                    spriteBatch.DrawString(_mainFont, upgradeInfoText,
                        equipmentButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 20), Color.Cyan);
                }
            }

            _equipmentHint?.Draw(spriteBatch);
        }

        private bool CheckUpgradeIsAvailable(Equipment equipment)
        {
            var resourceItem = _player.Inventory.Single(x => x.Type == equipment.Scheme.RequiredResourceToLevelUp);
            var equipmentResourceAmount = resourceItem.Amount;
            return equipmentResourceAmount >= equipment.RequiredResourceAmountToLevelUp;
        }

        private void ClearEquipmentHint()
        {
            _equipmentUnderHint = null;
            _equipmentHint = null;
        }

        private void CreateHint(EntityIconButton<Equipment> equipmentButton, Equipment equipment)
        {
            _equipmentUnderHint = equipment;

            var equipmentNameText = GameObjectHelper.GetLocalized(equipment.Scheme.Sid);
            var equipmentDescriptionText = GameObjectHelper.GetLocalizedDescription(equipment.Scheme.Sid);
            var resourceName = GameObjectHelper.GetLocalized(equipment.Scheme.RequiredResourceToLevelUp);
            var requiredResourceCount = equipment.RequiredResourceAmountToLevelUp;
            var upgradeInfoText =
                string.Format(UiResource.EquipmentResourceRequipmentTemplate, resourceName, requiredResourceCount);

            var sb = new StringBuilder();
            sb.AppendLine(equipmentNameText);
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(upgradeInfoText);
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(equipmentDescriptionText);

            _equipmentHint = new TextHint(_hintTexture, _mainFont, sb.ToString());
            _equipmntButtonUnderHint = equipmentButton;
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
            CreateHint((EntityIconButton<Equipment>)sender, equipment);
        }

        private static int? GetEquipmentIconOneBasedIndex(IEquipmentSchemeMetadata metadata)
        {
            if (metadata is EquipmentSchemeMetadata equipmentSchemeMetadata)
            {
                return equipmentSchemeMetadata.IconOneBasedIndex;
            }

            return null;
        }

        private static Rectangle GetEquipmentIconRect(IEquipmentSchemeMetadata? metadata)
        {
            if (metadata is null)
            {
                return new Rectangle(0, 0, ICON_SIZE, ICON_SIZE);
            }

            var index = GetEquipmentIconOneBasedIndex(metadata);
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
                        CreateHint(equipmentButton, equipmentButton.Entity);
                    }

                    if (_equipmentUnderHint != equipmentButton.Entity)
                    {
                        CreateHint(equipmentButton, equipmentButton.Entity);
                    }
                }
            }

            if (currentEquipment is null)
            {
                ClearEquipmentHint();
            }

            if (_equipmentHint is not null)
            {
                var textSize = _mainFont.MeasureString(_equipmentHint.Text);
                var marginVector = new Vector2(10, 15) * 2;
                var position = _equipmntButtonUnderHint.Rect.Location -
                               new Point(5, (int)(textSize.Y + marginVector.Y));
                _equipmentHint.Rect = new Rectangle(position, (textSize + marginVector).ToPoint());
            }
        }
    }
}