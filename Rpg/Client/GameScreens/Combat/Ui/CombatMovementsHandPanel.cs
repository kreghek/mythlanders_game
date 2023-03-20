using System;
using System.Diagnostics;

using Client.Assets.CombatMovements;
using Client.Engine;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Client.GameScreens.Combat.Ui;

internal class CombatMovementsHandPanel : ControlBase
{
    private const int ICON_SIZE = 64;
    private const int BUTTON_PADDING = 5;
    private const int BUTTON_MARGIN = 5;
    private const int SKILL_BUTTON_SIZE = ICON_SIZE + BUTTON_PADDING;
    private const int SKILL_SELECTION_OFFSET = SKILL_BUTTON_SIZE / 8;

    private readonly CombatMovementButton?[] _buttons;
    private readonly ICombatMovementVisualizer _combatMovementVisualizer;
    private readonly IUiContentStorage _uiContentStorage;

    private CombatMovementHint? _activeCombatMovementHint;
    private BurningCombatMovement? _burningCombatMovement;
    private Combatant? _combatant;
    private KeyboardState _currentKeyboardState;
    private EntityButtonBase<CombatMovementInstance>? _hoverButton;
    private KeyboardState? _lastKeyboardState;

    public CombatMovementsHandPanel(IUiContentStorage uiContentStorage,
        ICombatMovementVisualizer combatMovementVisualizer)
    {
        _buttons = new CombatMovementButton[3];

        _uiContentStorage = uiContentStorage;
        _combatMovementVisualizer = combatMovementVisualizer;
        IsEnabled = true;
    }

    public Combatant? Combatant
    {
        get => _combatant;
        set
        {
            _combatant = value;

            RecreateButtons();
        }
    }

    public bool IsEnabled { get; set; }

    public bool Readonly { get; set; }

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
        for (var buttonIndex = 0; buttonIndex < _buttons.Length; buttonIndex++)
        {
            var button = _buttons[buttonIndex];

            if (button is not null)
            {
                var isSelected = button.IsSelected;

                button.Rect = GetButtonRectangle(buttonsRect, buttonIndex, isSelected);
                button.Draw(spriteBatch);

                var hotKey = (buttonIndex + 1).ToString();
                DrawHotkey(spriteBatch, hotKey, button, isSelected);
            }
            else if (_burningCombatMovement is not null)
            {
                if (_burningCombatMovement.HandSlotIndex == buttonIndex)
                {
                    _burningCombatMovement.Rect = GetButtonRectangle(buttonsRect, buttonIndex, true);
                    _burningCombatMovement.Draw(spriteBatch);
                }
            }
        }

        if (_hoverButton is not null && _activeCombatMovementHint is not null)
        {
            DrawHoverInfo(_hoverButton, _activeCombatMovementHint, spriteBatch);
        }
    }

    internal void StartMovementBurning(int handSlotIndex)
    {
        var combatMovementButton = _buttons[handSlotIndex];
        _buttons[handSlotIndex] = null;
        if (combatMovementButton is not null)
        {
            _burningCombatMovement = new BurningCombatMovement(combatMovementButton.IconData, handSlotIndex);
        }
    }

    internal void Update(GameTime gameTime, ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        if (!IsEnabled)
        {
            return;
        }

        if (!Readonly)
        {
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
                if (button is not null)
                {
                    button.Update(resolutionIndependentRenderer);

                    DetectMouseHoverOnButton(mouseRect, button);
                }
            }

            if (_hoverButton is not null && _hoverButton != oldHoverButton && _combatant is not null)
            {
                _activeCombatMovementHint = new CombatMovementHint(_hoverButton.Entity);
            }
            else if (_hoverButton is not null && _hoverButton == oldHoverButton && _combatant is not null)
            {
                // Do nothing because hint of this button is created yet.
            }
            else
            {
                _activeCombatMovementHint = null;
            }
        }

        if (_burningCombatMovement is not null)
        {
            _burningCombatMovement.Update(gameTime);

            if (_burningCombatMovement.IsComplete)
            {
                _burningCombatMovement = null;
            }
        }
    }

    private void CombatMovementButton_OnClick(object? sender, EventArgs e)
    {
        if (sender is null)
        {
            Debug.Fail("Sender mustn't be null.");
        }

        var entityButton = (EntityButtonBase<CombatMovementInstance>)sender;

        CombatMovementPicked?.Invoke(this, new CombatMovementPickedEventArgs(entityButton.Entity));
    }

    private void DetectMouseHoverOnButton(Rectangle mouseRect, EntityButtonBase<CombatMovementInstance> button)
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

    private static void DrawHoverInfo(ControlBase baseControl, CombatMovementHint hintControl,
        SpriteBatch spriteBatch)
    {
        var baseControlCenter = baseControl.Rect.Center;
        var baseControlTopCenter = new Point(baseControlCenter.X, baseControl.Rect.Top);

        var hintSize = (hintControl.ContentSize + new Vector2(CONTENT_MARGIN * 2)).ToPoint();
        var hintHorizontalCenter = hintSize.X / 2;
        const int HINT_MARGIN = 5;

        var hintPosition = baseControlTopCenter - new Point(hintHorizontalCenter, hintSize.Y + HINT_MARGIN);
        var hintRectangle = new Rectangle(hintPosition, hintSize);

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
        var allButtonWidth = _buttons.Length * (SKILL_BUTTON_SIZE + BUTTON_MARGIN);
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
        if (index < 0 || _buttons.Length <= index)
        {
            return;
        }

        var button = _buttons[index];

        if (button is null)
        {
            return;
        }

        button.Click();
    }

    private void RecreateButtons()
    {
        if (_combatant is null)
        {
            return;
            //throw new InvalidOperationException("Unit required to be initialized before.");
        }

        for (var buttonIndex = 0; buttonIndex < _combatant.Hand.Count; buttonIndex++)
        {
            var movement = _combatant.Hand[buttonIndex];
            if (movement is not null)
            {
                var icon = _combatMovementVisualizer.GetMoveIcon(movement.SourceMovement.Sid);
                var iconRect = UnsortedHelpers.GetIconRect(icon);
                var iconData = new IconData(_uiContentStorage.GetCombatMoveIconsTexture(), iconRect);
                var button = new CombatMovementButton(iconData, movement);
                _buttons[buttonIndex] = button;
                button.OnClick += CombatMovementButton_OnClick;
            }
            else
            {
                _buttons[buttonIndex] = null;
            }
        }
    }

    public event EventHandler<CombatMovementPickedEventArgs>? CombatMovementPicked;
}