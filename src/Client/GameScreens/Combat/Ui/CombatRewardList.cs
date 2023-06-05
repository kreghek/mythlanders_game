using System;
using System.Collections.Generic;
using System.Linq;

using Client;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class CombatRewardList : ControlBase
    {
        private const int MARGIN = 5;
        private readonly Texture2D _rewardIconsTexture;

        private readonly IReadOnlyCollection<AnimatedCountableUnitItemStat> _rewardItems;
        private readonly SpriteFont _textFont;
        private readonly SpriteFont _titleFont;

        public CombatRewardList(
            Texture2D propIconsTexture,
            IReadOnlyCollection<AnimatedCountableUnitItemStat> rewardItems)
        {
            _titleFont = UiThemeManager.UiContentStorage.GetTitlesFont();
            _textFont = UiThemeManager.UiContentStorage.GetMainFont();
            _rewardIconsTexture = propIconsTexture;
            _rewardItems = rewardItems;
        }

        public void Update()
        {
            foreach (var item in _rewardItems)
            {
                item.Update();
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

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            // Do not draw background
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            const int TITLE_HEIGHT = 20; // See title control

            const int REWARD_BLOCK_HEIGHT = 20;

            var listRect = new Rectangle(contentRect.X, contentRect.Y + TITLE_HEIGHT, contentRect.Width,
                REWARD_BLOCK_HEIGHT);

            DrawRewardList(spriteBatch, _rewardItems.ToArray(), listRect);
        }

        private void DrawRewardList(SpriteBatch spriteBatch, AnimatedCountableUnitItemStat[] rewardItems,
            Rectangle contentRectangle)
        {
            const int TITLE_OFFSET = 20; // Title here is the label "Found items"
            const int ITEM_WIDTH = 128;
            const int ITEM_HEIGHT = 32;
            const int CELL_COLS = 3;

            spriteBatch.DrawString(_titleFont, UiResource.CombatResultItemsFoundLabel,
                contentRectangle.Location.ToVector2(),
                Color.White);

            var orderedRewardItems = rewardItems.OrderBy(x => x.Type).ToArray();

            for (var itemIndex = 0; itemIndex < orderedRewardItems.Length; itemIndex++)
            {
                var item = orderedRewardItems[itemIndex];

                var itemCellX = itemIndex % CELL_COLS;
                var itemCellY = itemIndex / CELL_COLS;

                var itemOffsetVector =
                    new Vector2(itemCellX * (ITEM_WIDTH + MARGIN), (ITEM_HEIGHT + MARGIN) * itemCellY + TITLE_OFFSET);

                var rewardItemPosition = contentRectangle.Location.ToVector2() + itemOffsetVector;

                var resourceIconRect = GetEquipmentSpriteRect(item.Type);

                spriteBatch.Draw(_rewardIconsTexture, rewardItemPosition, resourceIconRect,
                    Color.White);

                var localizedName = GameObjectHelper.GetLocalized(item.Type);

                spriteBatch.DrawString(_textFont,
                    $"{localizedName}{Environment.NewLine}x{item.CurrentValue} (+{item.Amount})",
                    rewardItemPosition + new Vector2(32 + MARGIN, 0),
                    Color.Wheat);
            }
        }

        private static int GetEquipmentSpriteIndex(EquipmentItemType equipmentItemType)
        {
            return (int)equipmentItemType;
        }

        private static Rectangle GetEquipmentSpriteRect(EquipmentItemType equipmentItemType)
        {
            const int COLUMN_COUNT = 2;
            const int ICON_SIZE = 32;

            var index = GetEquipmentSpriteIndex(equipmentItemType);

            var x = index % COLUMN_COUNT;
            var y = index / COLUMN_COUNT;

            return new Rectangle(x * ICON_SIZE, y * ICON_SIZE, ICON_SIZE, ICON_SIZE);
        }
    }
}