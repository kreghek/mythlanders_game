using System;
using System.Linq;

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

        private readonly CombatItem _rewardItems;
        private readonly SpriteFont _textFont;
        private readonly SpriteFont _titleFont;

        public CombatRewardList(Texture2D texture,
            SpriteFont titleFont,
            SpriteFont textFont,
            Texture2D rewardIconsTexture,
            CombatItem? combatItemsLocal) : base(texture)
        {
            _titleFont = titleFont;
            _textFont = textFont;
            _rewardIconsTexture = rewardIconsTexture;
            _rewardItems = combatItemsLocal;
        }

        public void Update()
        {
            _rewardItems.Update();
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
            const int TITLE_HEIGHT = 20;

            spriteBatch.DrawString(_titleFont, UiResource.CombatResultItemsFoundLabel, contentRect.Location.ToVector2(),
                Color.White);

            var listRect = new Rectangle(contentRect.X, contentRect.Y + TITLE_HEIGHT, contentRect.Width,
                contentRect.Height - TITLE_HEIGHT);
            DrawRewardList(spriteBatch, _rewardItems.UnitItems.ToArray(), listRect);
        }

        private void DrawRewardList(SpriteBatch spriteBatch, AnimatedRewardItem[] rewardItems,
            Rectangle contentRectangle)
        {
            const int ITEM_WIDTH = 128;
            const int ITEM_HEIGHT = 32;
            const int CELL_COLS = 3;

            var orderedRewardItems = rewardItems.OrderBy(x => x.Equipment.Type).ToArray();

            for (var itemIndex = 0; itemIndex < orderedRewardItems.Length; itemIndex++)
            {
                var item = orderedRewardItems[itemIndex];

                var itemCellX = itemIndex % CELL_COLS;
                var itemCellY = itemIndex / CELL_COLS;

                var itemOffsetVector =
                    new Vector2(itemCellX * (ITEM_WIDTH + MARGIN), (ITEM_HEIGHT + MARGIN) * itemCellY);

                var rewardItemPosition = contentRectangle.Location.ToVector2() + itemOffsetVector;

                var resourceIconRect = GetEquipmentSpriteRect(item.Equipment.Type);

                spriteBatch.Draw(_rewardIconsTexture, rewardItemPosition, resourceIconRect,
                    Color.White);

                var localizedName = GameObjectHelper.GetLocalized(item.Equipment.Type);

                spriteBatch.DrawString(_textFont, $"{localizedName}{Environment.NewLine}x{item.Equipment.CurrentValue}",
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