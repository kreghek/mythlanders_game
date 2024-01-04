using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Client.Assets.Equipments;
using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.Barracks.Ui;

internal sealed class EquipmentsInfoPanel : PanelBase
{
    private const int ICON_SIZE = 64;
    private const int MARGIN = 5;

    private readonly IList<EntityIconButton<Equipment>> _equipmentButtons;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;
    private EntityIconButton<Equipment>? _equipmentButtonUnderHint;
    private TextHint? _equipmentHint;
    private Equipment? _equipmentUnderHint;

    public EquipmentsInfoPanel(HeroState hero, Texture2D equipmentIconsTexture,
        IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
        _equipmentButtons = new List<EntityIconButton<Equipment>>();

        InitEquipmentButtons(hero, equipmentIconsTexture);
    }

    protected override string TitleResourceId => nameof(UiResource.HeroEquipmentInfoTitle);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        HandleEquipmentHint();

        foreach (var equipmentButton in _equipmentButtons)
        {
            equipmentButton.IsEnabled = true;

            equipmentButton.Update(_resolutionIndependentRenderer);
        }
    }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var mainFont = UiThemeManager.UiContentStorage.GetMainFont();

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

            spriteBatch.DrawString(mainFont, entityInfoText,
                equipmentButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 0), Color.Wheat);
        }

        _equipmentHint?.Draw(spriteBatch);
    }

    private void ClearEquipmentHint()
    {
        _equipmentUnderHint = null;
        _equipmentHint = null;
    }

    private EntityIconButton<Equipment> CreateEquipmentButton(Equipment equipment,
        Texture2D equipmentIconsTexture)
    {
        var equipmentIconRect = GetEquipmentIconRect(equipment.Scheme.Metadata);

        var equipmentIconButton = new EntityIconButton<Equipment>(
            new IconData(equipmentIconsTexture, equipmentIconRect), equipment);
        return equipmentIconButton;
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

        _equipmentHint = new TextHint(sb.ToString());
        _equipmentButtonUnderHint = equipmentButton;
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
            var textSize = UiThemeManager.UiContentStorage.GetMainFont().MeasureString(_equipmentHint.Text);
            var marginVector = new Vector2(10, 15) * 2;

            Debug.Assert(_equipmentButtonUnderHint is not null,
                "_equipmentButtonUnderHint always assigned with _equipmentHint");

            var position = _equipmentButtonUnderHint.Rect.Location -
                           new Point(5, (int)(textSize.Y + marginVector.Y));
            _equipmentHint.Rect = new Rectangle(position, (textSize + marginVector).ToPoint());
        }
    }

    private void InitEquipmentButtons(HeroState hero, Texture2D equipmentIconsTexture)
    {
        var equipmentList = hero.Equipments;
        foreach (var equipment in equipmentList)
        {
            var equipmentIconButton =
                CreateEquipmentButton(equipment, equipmentIconsTexture);

            _equipmentButtons.Add(equipmentIconButton);
        }
    }
}