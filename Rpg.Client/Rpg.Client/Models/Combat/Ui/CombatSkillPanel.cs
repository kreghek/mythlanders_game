using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class CombatSkillPanel
    {
        private readonly IList<IconButton> _buttons;
        private readonly IUiContentStorage _uiContentStorage;
        private CombatSkillCard _selectedCard;
        private CombatUnit? _unit;

        public CombatSkillPanel(IUiContentStorage uiContentStorage)
        {
            _buttons = new List<IconButton>();
            _uiContentStorage = uiContentStorage;

            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }

        public CombatSkillCard? SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (_selectedCard == value)
                {
                    return;
                }

                _selectedCard = value;

                CardSelected?.Invoke(this, _selectedCard);
            }
        }

        public CombatUnit? Unit
        {
            get => _unit;
            set
            {
                if (_unit == value)
                {
                    return;
                }

                _unit = value;

                RecreateButtons();
            }
        }

        internal void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (!IsEnabled)
            {
                return;
            }

            var buttonWidth = _buttons.Count * 32;
            for (var buttonIndex = 0; buttonIndex < _buttons.Count; buttonIndex++)
            {
                var button = _buttons[buttonIndex];
                button.Rect = GetButtonRectangle(graphicsDevice, buttonWidth, buttonIndex);
                button.Draw(spriteBatch);
            }
        }

        internal void Update()
        {
            if (!IsEnabled)
            {
                return;
            }

            foreach (var button in _buttons)
            {
                button.Update();
            }
        }

        private static Rectangle GetButtonRectangle(GraphicsDevice graphicsDevice, int buttonWidth, int i)
        {
            return new Rectangle(graphicsDevice.Viewport.Bounds.Center.X - buttonWidth / 2 + 32 * i,
                graphicsDevice.Viewport.Bounds.Bottom - 32, 32, 32);
        }

        private static int? GetIconIndex(string sid)
        {
            return sid switch
            {
                "Slash" => 0,
                "Wide Slash" => 1,
                "Strike" => 2,
                "Arrow Rain" => 3,
                "Heal" => 4,
                "Mass Heal" => 5,
                _ => null
            };
        }

        private static Rectangle GetIconRect(string sid)
        {
            const int SPRITESHEET_COLUMN_COUNT = 2;
            const int ICON_SIZE = 32;

            var iconIndexNullable = GetIconIndex(sid);

            Debug.Assert(iconIndexNullable is not null, "Don't forget add combat power in GetIconIndex");

            var iconIndex = iconIndexNullable is not null ? iconIndexNullable.Value : 0;

            var x = iconIndex % SPRITESHEET_COLUMN_COUNT;
            var y = iconIndex / SPRITESHEET_COLUMN_COUNT;
            var rect = new Rectangle(x * ICON_SIZE, y * ICON_SIZE, ICON_SIZE, ICON_SIZE);

            return rect;
        }

        private void RecreateButtons()
        {
            _buttons.Clear();
            SelectedCard = null;

            if (_unit is null)
            {
                SelectedCard = null;
                return;
                //throw new InvalidOperationException("Unit required to be initialized before.");
            }

            if (_unit.CombatCards is null)
            {
                throw new InvalidOperationException($"The unit {_unit} required to have got combat powers.");
            }

            foreach (var card in _unit.CombatCards)
            {
                var iconRect = GetIconRect(card.Skill.Sid);
                var iconData = new IconData(_uiContentStorage.GetCombatPowerIconsTexture(), iconRect);
                var button = new IconButton(_uiContentStorage.GetButtonTexture(), iconData, Rectangle.Empty);
                _buttons.Add(button);
                button.OnClick += (s, e) =>
                {
                    SelectedCard = card;
                };
            }
        }

        public event EventHandler<CombatSkillCard?>? CardSelected;
    }
}