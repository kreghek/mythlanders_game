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
    internal class CombatSkillPanel : ISkillPanelState
    {
        private const int ICON_SIZE = 64;
        private const int BUTTON_PADDING = 5;
        private const int BUTTON_MARGIN = 5;
        private const int SKILL_BUTTON_SIZE = ICON_SIZE + BUTTON_PADDING;
        private const int SPRITE_SHEET_COLUMN_COUNT = 4;
        private readonly IDictionary<ButtonBase, CombatSkill> _buttonCombatPowerDict;
        private readonly IList<ButtonBase> _buttons;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private SkillHint? _activeSkillHint;
        private KeyboardState _currentKeyboardState;

        private ButtonBase? _hoverButton;
        private KeyboardState? _lastKeyboardState;
        private CombatSkill? _selectedCard;
        private CombatUnit? _unit;

        public CombatSkillPanel(IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _buttons = new List<ButtonBase>();
            _buttonCombatPowerDict = new Dictionary<ButtonBase, CombatSkill>();

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

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!IsEnabled)
            {
                return;
            }

            var panelWidth = _buttons.Count * SKILL_BUTTON_SIZE;
            for (var buttonIndex = 0; buttonIndex < _buttons.Count; buttonIndex++)
            {
                var button = _buttons[buttonIndex];
                button.Rect = GetButtonRectangle(_resolutionIndependentRenderer, panelWidth, buttonIndex);
                button.Draw(spriteBatch);

                var hotKey = (buttonIndex + 1).ToString();
                DrawHotkey(spriteBatch, hotKey, button);
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

                var combatSkill = _buttonCombatPowerDict[button];
                button.IsEnabled = combatSkill.IsAvailable;

                DetectMouseHoverOnButton(mouseRect, button);
            }

            if (_hoverButton is not null && _hoverButton != oldHoverButton && _unit is not null)
            {
                _activeSkillHint = new SkillHint(_uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(),
                    _buttonCombatPowerDict[_hoverButton], _unit);
            }
            else if (_hoverButton is not null && _hoverButton == oldHoverButton && _unit is not null)
            {
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

            var combatPowerCard = _buttonCombatPowerDict[(ButtonBase)sender];
            SelectedCard = combatPowerCard;
        }

        private void DetectMouseHoverOnButton(Rectangle mouseRect, ButtonBase button)
        {
            if (mouseRect.Intersects(button.Rect))
            {
                _hoverButton = button;
            }
        }

        private void DrawHotkey(SpriteBatch spriteBatch, string hotKey, ControlBase button)
        {
            var hotkeyPosition = new Vector2(button.Rect.Center.X, button.Rect.Top) - new Vector2(0, 15);
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

        private static Rectangle GetButtonRectangle(ResolutionIndependentRenderer resolutionIndependentRenderer,
            int panelWidth, int buttonIndex)
        {
            var panelMiddleX = panelWidth / 2;
            var buttonOffsetX = (SKILL_BUTTON_SIZE + BUTTON_MARGIN) * buttonIndex;
            var panelLeftX = resolutionIndependentRenderer.VirtualBounds.Center.X - panelMiddleX;

            return new Rectangle(panelLeftX + buttonOffsetX,
                resolutionIndependentRenderer.VirtualBounds.Bottom - SKILL_BUTTON_SIZE,
                SKILL_BUTTON_SIZE,
                SKILL_BUTTON_SIZE);
        }

        private static int? GetIconIndex(SkillSid sid)
        {
            return sid switch
            {
                SkillSid.SwordSlash => 0,
                SkillSid.DefenseStance => 1,
                SkillSid.WideSwordSlash => 2,
                SkillSid.SvarogBlastFurnace => 2,
                SkillSid.EnergyShot => 3,
                SkillSid.RapidEnergyShot => 3,
                SkillSid.ArrowRain => 4,
                SkillSid.ZduhachMight => 5,
                SkillSid.Heal => 6,
                SkillSid.ToxicHerbs => 9,
                SkillSid.HealingSalve => 6,
                SkillSid.DopeHerb => 7,
                SkillSid.MassHeal => 8,

                SkillSid.StaffHit => 12,
                SkillSid.HealMantre => 13,
                SkillSid.PathOf1000Firsts => 14,

                SkillSid.DarkLight => 9,
                SkillSid.ParaliticChor => 10,
                SkillSid.FingerOfAnubis => 11,

                SkillSid.PowerUp => 1,
                _ => null
            };
        }

        private static Rectangle GetIconRect(SkillSid sid)
        {
            var iconIndexNullable = GetIconIndex(sid);

            Debug.Assert(iconIndexNullable is not null, $"Don't forget add combat power in {nameof(GetIconIndex)}");

            var iconIndex = iconIndexNullable.GetValueOrDefault();

            var x = iconIndex % SPRITE_SHEET_COLUMN_COUNT;
            var y = iconIndex / SPRITE_SHEET_COLUMN_COUNT;
            var rect = new Rectangle(x * ICON_SIZE, y * ICON_SIZE, ICON_SIZE, ICON_SIZE);

            return rect;
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
            _buttonCombatPowerDict.Clear();
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
                var button = new CombatSkillButton(_uiContentStorage.GetButtonTexture(), iconData, Rectangle.Empty,
                    card, this);
                _buttons.Add(button);
                _buttonCombatPowerDict[button] = card;
                button.OnClick += CombatPowerButton_OnClick;
            }
        }

        public CombatSkill? SelectedCard
        {
            get => _selectedCard;
            set
            {
                // Comment this block because monsters can transforms into other unit (See scheme auto transition).
                // But interaction buttons keep reference to old unit. This is reason of a errors.
                /*if (_selectedCard == value)
                {
                    return;
                }*/

                _selectedCard = value;

                CardSelected?.Invoke(this, _selectedCard);
            }
        }

        public event EventHandler<CombatSkill?>? CardSelected;
    }
}