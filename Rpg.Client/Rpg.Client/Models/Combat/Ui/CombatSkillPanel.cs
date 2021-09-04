namespace Rpg.Client.Models.Combat.Ui
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Rpg.Client.Core;
    using Rpg.Client.Engine;

    internal class CombatSkillPanel
    {
        private const int BUTTON_SIZE = 32;

        private readonly IList<IconBaseButton> _buttons;

        private readonly int _panelWidth;

        private readonly IUiContentStorage _uiContentStorage;

        private CombatUnit? _unit;

        public CombatSkillPanel(IUiContentStorage uiContentStorage)
        {
            _buttons = new List<IconBaseButton>();
            _uiContentStorage = uiContentStorage;
            _panelWidth = _buttons.Count * BUTTON_SIZE;
        }

        public CombatSkillCard? SelectedCard { get; private set; }

        public CombatUnit? Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                RefreshButtons();
            }
        }

        internal void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            for (var i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                button.Rect = GetButtonRectangle(graphicsDevice: graphicsDevice, buttonWidth: _panelWidth, buttonIndex: i);
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

        private static Rectangle GetButtonRectangle(GraphicsDevice graphicsDevice, int buttonWidth, int buttonIndex)
        {
            var offsetPanel = graphicsDevice.Viewport.Bounds.Bottom / 10;
            var buttonYPosition = graphicsDevice.Viewport.Bounds.Bottom - offsetPanel;

            return new Rectangle(
                x: graphicsDevice.Viewport.Bounds.Center.X - buttonWidth / 2 + BUTTON_SIZE * buttonIndex,
                y: buttonYPosition,
                width: BUTTON_SIZE,
                height: BUTTON_SIZE);
        }

        private void RefreshButtons()
        {
            _buttons.Clear();
            foreach (var card in _unit.CombatCards)
            {
                var button = new IconBaseButton(
                    texture: _uiContentStorage.GetButtonTexture(),
                    icon: _uiContentStorage.GetButtonTexture(),
                    rect: new Rectangle(x: 0, y: 0, width: 0, height: 0));
                _buttons.Add(button);
                button.OnClick += (s, e) => { SelectedCard = card; };
            }
        }
    }
}