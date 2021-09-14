using System;
using System.Collections.Generic;

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
        }

        public CombatSkillCard? SelectedCard
        {
            get
            {
                return _selectedCard;
            }
            set
            {
                if (_selectedCard == value)
                    return;

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

                RefreshButtons();
            }
        }

        internal void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
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

        private void RefreshButtons()
        {
            _buttons.Clear();
            SelectedCard = null;

            if (_unit is null)
                return;
            //{
            //    throw new InvalidOperationException("Unit required to be initialized before.");
            //}

            if (_unit.CombatCards is null)
            {
                throw new InvalidOperationException($"The unit {_unit} required to have got combat powers.");
            }

            foreach (var card in _unit.CombatCards)
            {
                var button = new IconButton(_uiContentStorage.GetButtonTexture(), _uiContentStorage.GetButtonTexture(),
                    new Rectangle(0, 0, 0, 0));
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