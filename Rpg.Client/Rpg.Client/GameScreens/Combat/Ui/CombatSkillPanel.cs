using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class CombatSkillPanel : ControlBase, ISkillPanelState
    {
        private const int ICON_SIZE = 64;
        private const int BUTTON_PADDING = 5;
        private const int BUTTON_MARGIN = 5;
        private const int SKILL_BUTTON_SIZE = ICON_SIZE + BUTTON_PADDING;
        private const int SKILL_SELECTION_OFFSET = SKILL_BUTTON_SIZE / 8;

        private readonly IList<EntityButtonBase<CombatSkill>> _buttons;
        private readonly IUiContentStorage _uiContentStorage;
        private SkillHint? _activeSkillHint;
        private KeyboardState _currentKeyboardState;

        private EntityButtonBase<CombatSkill>? _hoverButton;
        private KeyboardState? _lastKeyboardState;
        private CombatSkill? _selectedSkill;
        private CombatUnit? _unit;

        public CombatSkillPanel(Texture2D texture, IUiContentStorage uiContentStorage) : base(texture)
        {
            _buttons = new List<EntityButtonBase<CombatSkill>>();

            _uiContentStorage = uiContentStorage;

            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }

        public CombatUnit? Unit
        {
            get => _unit;
            set
            {
                // Comment this block because monsters can transforms into other unit (See scheme auto transition).
                // But interaction buttons keep reference to old unit. This is reason of a errors.
                /*if (_unit == value)
                {
                    return;
                }*/

                _unit = value;

                RecreateButtons();
            }
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            var buttonsRect = GetButtonsRect();

            const int IMAGE_WIDTH = 480;
            const int IMAGE_HEIGHT = 43;
            var leftPartRect = new Rectangle(buttonsRect.Left - IMAGE_WIDTH / 2, Rect.Center.Y - IMAGE_HEIGHT / 2,
                IMAGE_WIDTH / 2, IMAGE_HEIGHT);
            var rightPartRect = new Rectangle(buttonsRect.Right, Rect.Center.Y - IMAGE_HEIGHT / 2, IMAGE_WIDTH / 2,
                IMAGE_HEIGHT);

            spriteBatch.Draw(_uiContentStorage.GetCombatSkillPanelTexture(),
                new Rectangle(leftPartRect.Right, leftPartRect.Top, rightPartRect.Left - leftPartRect.Right,
                    leftPartRect.Height),
                new Rectangle(IMAGE_WIDTH / 2 - 1, 0, 2, IMAGE_HEIGHT),
                color);

            spriteBatch.Draw(_uiContentStorage.GetCombatSkillPanelTexture(),
                leftPartRect,
                new Rectangle(0, 0, IMAGE_WIDTH / 2, IMAGE_HEIGHT),
                color);

            spriteBatch.Draw(_uiContentStorage.GetCombatSkillPanelTexture(),
                rightPartRect,
                new Rectangle(IMAGE_WIDTH / 2, 0, IMAGE_WIDTH / 2, IMAGE_HEIGHT),
                color);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            if (!IsEnabled)
            {
                return;
            }

            var buttonsRect = GetButtonsRect();
            for (var buttonIndex = 0; buttonIndex < _buttons.Count; buttonIndex++)
            {
                var button = _buttons[buttonIndex];
                var isSelected = SelectedSkill == button.Entity;

                button.Rect = GetButtonRectangle(buttonsRect, buttonIndex, isSelected);
                button.Draw(spriteBatch);

                var hotKey = (buttonIndex + 1).ToString();
                DrawHotkey(spriteBatch, hotKey, button, isSelected);
            }

            if (_hoverButton is not null && _activeSkillHint is not null)
            {
                DrawHoverCombatSkillInfo(_hoverButton, _activeSkillHint, spriteBatch);
            }
        }

        internal void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            if (!IsEnabled)
            {
                return;
            }

            KeyboardInputUpdate();

            var mouse = Mouse.GetState();
            var mouseRect =
                new Rectangle(
                    resolutionIndependentRenderer.ScaleMouseToScreenCoordinates(mouse.Position.ToVector2()).ToPoint(),
                    new Point(1, 1));

            var oldHoverButton = _hoverButton;
            _hoverButton = null;
            foreach (var button in _buttons)
            {
                button.Update(resolutionIndependentRenderer);

                DetectMouseHoverOnButton(mouseRect, button);
            }

            if (_hoverButton is not null && _hoverButton != oldHoverButton && _unit is not null)
            {
                _activeSkillHint = new SkillHint(_uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(),
                    _hoverButton.Entity, _unit);
            }
            else if (_hoverButton is not null && _hoverButton == oldHoverButton && _unit is not null)
            {
                // Do nothing because hint of this button is created yet.
            }
            else
            {
                _activeSkillHint = null;
            }
        }

        private void CombatPowerButton_OnClick(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                Debug.Fail("Sender mustn't be null.");
            }

            var entityButton = (EntityButtonBase<CombatSkill>)sender;
            SelectedSkill = entityButton.Entity;
        }

        private void DetectMouseHoverOnButton(Rectangle mouseRect, EntityButtonBase<CombatSkill> button)
        {
            if (mouseRect.Intersects(button.Rect))
            {
                _hoverButton = button;
            }
        }

        private void DrawHotkey(SpriteBatch spriteBatch, string hotKey, ControlBase button, bool isSelected)
        {
            var font = _uiContentStorage.GetMainFont();
            var textSize = font.MeasureString(hotKey);
            var marginOffset = new Vector2(0, 2);
            var textOffset = new Vector2(textSize.X / 2, textSize.Y);
            var selectedOffset = isSelected ? new Vector2(0, SKILL_SELECTION_OFFSET) : Vector2.Zero;
            var hotkeyPosition = new Vector2(button.Rect.Center.X, button.Rect.Top) -
                                 (textOffset + marginOffset + selectedOffset);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), hotKey, hotkeyPosition, Color.Wheat);
        }

        private static void DrawHoverCombatSkillInfo(ControlBase baseControl, ControlBase hintControl,
            SpriteBatch spriteBatch)
        {
            var baseControlCenter = baseControl.Rect.Center;
            var baseControlTopCenter = new Point(baseControlCenter.X, baseControl.Rect.Top);

            // TODO Calculate preferred size of the hint based on content.
            var hintWidth = 200;
            var hintHeight = 75;
            var hintHorizontalCenter = hintWidth / 2;
            const int HINT_MARGIN = 5;

            var hintPosition = baseControlTopCenter - new Point(hintHorizontalCenter, hintHeight + HINT_MARGIN);
            var hintRectangle = new Rectangle(hintPosition, new Point(hintWidth, hintHeight));

            hintControl.Rect = hintRectangle;
            hintControl.Draw(spriteBatch);
        }

        private static Rectangle GetButtonRectangle(Rectangle buttonsRect, int buttonIndex, bool isSelected)
        {
            var buttonOffsetX = (SKILL_BUTTON_SIZE + BUTTON_MARGIN) * buttonIndex;
            var buttonOffsetY = isSelected ? -SKILL_BUTTON_SIZE / 8 : 0;

            return new Rectangle(buttonsRect.X + buttonOffsetX,
                buttonsRect.Y + buttonOffsetY,
                SKILL_BUTTON_SIZE,
                SKILL_BUTTON_SIZE);
        }

        private Rectangle GetButtonsRect()
        {
            var allButtonWidth = _buttons.Count * (SKILL_BUTTON_SIZE + BUTTON_MARGIN);
            var buttonsRect =
                new Rectangle(Rect.Center.X - allButtonWidth / 2, Rect.Y, allButtonWidth, Rect.Height);
            return buttonsRect;
        }

        private bool IsKeyPressed(Keys key)
        {
            if (_lastKeyboardState is null)
            {
                return false;
            }

            return _lastKeyboardState.Value.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
        }

        private void KeyboardInputUpdate()
        {
            _currentKeyboardState = Keyboard.GetState();

            var buttonIndex = -1;

            if (IsKeyPressed(Keys.D1))
            {
                buttonIndex = 0;
            }
            else if (IsKeyPressed(Keys.D2))
            {
                buttonIndex = 1;
            }
            else if (IsKeyPressed(Keys.D3))
            {
                buttonIndex = 2;
            }
            else if (IsKeyPressed(Keys.D4))
            {
                buttonIndex = 3;
            }
            else if (IsKeyPressed(Keys.D5))
            {
                buttonIndex = 4;
            }

            PressButton(buttonIndex);

            _lastKeyboardState = _currentKeyboardState;
        }

        private void PressButton(int index)
        {
            if (index < 0 || _buttons.Count <= index)
            {
                return;
            }

            _buttons[index].Click();
        }

        private void RecreateButtons()
        {
            _buttons.Clear();
            SelectedSkill = null;

            if (_unit is null)
            {
                SelectedSkill = null;
                return;
                //throw new InvalidOperationException("Unit required to be initialized before.");
            }

            if (_unit.CombatCards is null)
            {
                throw new InvalidOperationException($"The unit {_unit} required to have got combat powers.");
            }

            foreach (var card in _unit.CombatCards)
            {
                var iconRect = UnsortedHelpers.GetIconRect(card.Skill.Sid);
                var iconData = new IconData(_uiContentStorage.GetCombatPowerIconsTexture(), iconRect);
                var button = new CombatSkillButton<CombatSkill>(_uiContentStorage.GetSkillButtonTexture(), iconData,
                    card, this);
                _buttons.Add(button);
                button.OnClick += CombatPowerButton_OnClick;
            }
        }

        public CombatSkill? SelectedSkill
        {
            get => _selectedSkill;
            set
            {
                // Comment this block because monsters can transforms into other unit (See scheme auto transition).
                // But interaction buttons keep reference to old unit. This is reason of a errors.
                /*if (_selectedCard == value)
                {
                    return;
                }*/

                _selectedSkill = value;

                SkillSelected?.Invoke(this, _selectedSkill);
            }
        }

        public event EventHandler<CombatSkill?>? SkillSelected;
    }
}