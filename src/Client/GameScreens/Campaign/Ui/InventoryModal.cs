using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Client.Core;
using Client.Engine;

using Core.Props;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Campaign.Ui;

internal class InventoryModal : ModalDialogBase
{
    private const int EQUIPMENT_ITEM_SIZE = 32;
    private const int EQUIPMENT_ITEM_SPACING = 2;
    private const int MAX_INVENTORY_ROWS = 8;

    private readonly Inventory _inventory;
    private readonly IUiContentStorage _uiContentStorage;

    private InventoryUiItem[]? _currentInventoryItems;

    public InventoryModal(Inventory inventory, IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer) : base(uiContentStorage,
        resolutionIndependentRenderer)
    {
        _inventory = inventory;
        _uiContentStorage = uiContentStorage;
    }

    public EventHandler PropButton_OnClick { get; }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        DrawInventory(spriteBatch);
    }

    protected override void InitContent()
    {
        base.InitContent();

        InitInventory(_inventory, ContentRect);
    }

    private void DrawInventory(SpriteBatch spriteBatch)
    {
        var rect = ContentRect;

        var currentInventoryItemList = new List<InventoryUiItem>();
        var inventoryItems = _inventory.CalcActualItems().ToArray();
        for (var itemIndex = 0; itemIndex < inventoryItems.Length; itemIndex++)
        {
            var prop = inventoryItems[itemIndex];
            if (prop is null)
            {
                continue;
            }

            var relativeY = itemIndex * (EQUIPMENT_ITEM_SIZE + EQUIPMENT_ITEM_SPACING);
            var buttonRect = new Rectangle(
                rect.Left,
                rect.Top + relativeY,
                EQUIPMENT_ITEM_SIZE,
                EQUIPMENT_ITEM_SIZE);

            var sid = prop.Scheme.Sid;
            if (string.IsNullOrEmpty(sid))
            {
                Debug.Fail("All prop must have symbolic identifier (SID).");
                sid = "EmptyPropIcon";
            }

            var propButton =
                new IconButton(new IconData(_uiContentStorage.GetEquipmentTextures(), new Rectangle(0, 0, 32, 32)));
            propButton.OnClick += PropButton_OnClick;

            var uiItem = new InventoryUiItem(propButton, prop, itemIndex, buttonRect);

            currentInventoryItemList.Add(uiItem);
        }

        _currentInventoryItems = currentInventoryItemList.ToArray();

        var pagedItems = _currentInventoryItems.Take(MAX_INVENTORY_ROWS).ToArray();

        foreach (var item in pagedItems)
        {
            item.Control.Rect = item.UiRect;
            item.Control.Draw(spriteBatch);

            var propTitle = GameObjectHelper.GetLocalizedProp(item.Prop.Scheme.Sid);
            if (item.Prop is Resource resource)
            {
                propTitle += $" x{resource.Count}";
            }

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), propTitle,
                new Vector2(item.Control.Rect.Right + 2, item.Control.Rect.Top), Color.Wheat);
        }
    }

    private void InitInventory(Inventory inventory, Rectangle rect)
    {
    }

    private record InventoryUiItem
    {
        public InventoryUiItem(IconButton control, IProp prop, int uiIndex, Rectangle uiRect)
        {
            UiIndex = uiIndex;
            UiRect = uiRect;
            Prop = prop ?? throw new ArgumentNullException(nameof(prop));
            Control = control ?? throw new ArgumentNullException(nameof(control));
        }

        public IconButton Control { get; }
        public IProp Prop { get; }

        public int UiIndex { get; }
        public Rectangle UiRect { get; }
    }
}