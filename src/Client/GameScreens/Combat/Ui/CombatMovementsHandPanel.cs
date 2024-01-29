using System;
using System.Diagnostics;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Engine;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Engine.Ui;

using GameAssets.Combats;

using GameClient.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.Combat.Ui;

internal class CombatMovementsHandPanel : ControlBase
{
    private const int ICON_SIZE = 64;
    private const int BUTTON_PADDING = 5;
    private const int BUTTON_MARGIN = 5;
    private const int SKILL_BUTTON_SIZE = ICON_SIZE + BUTTON_PADDING;
    private const int SKILL_SELECTION_OFFSET = SKILL_BUTTON_SIZE / 8;
    private const int SPECIAL_BUTTONS_ICON_WIDTH = 16;
    private const int SPECIAL_BUTTONS_ICON_HEIGHT = 32;

    private readonly CombatMovementButton?[] _buttons;
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizer;

    private readonly HoverController<CombatMovementButton> _hoverController;
    private readonly IUiContentStorage _uiContentStorage;
    private readonly WaitIconButton _waitButton;

    private CombatMovementHint? _activeCombatMovementHint;
    private BurningCombatMovement? _burningCombatMovement;
    private ICombatant? _combatant;

    private double _counterTacticalPotentialExhaustedIndicator;
    private KeyboardState _currentKeyboardState;
    private KeyboardState? _lastKeyboardState;

    public CombatMovementsHandPanel(
        Texture2D verticalButtonIcons,
        IUiContentStorage uiContentStorage,
        ICombatMovementVisualizationProvider combatMovementVisualizer) : base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
        _buttons = new CombatMovementButton[3];

        _uiContentStorage = uiContentStorage;
        _combatMovementVisualizer = combatMovementVisualizer;
        IsEnabled = true;

        _waitButton =
            new WaitIconButton(new IconData(verticalButtonIcons,
                new Rectangle(
                    SPECIAL_BUTTONS_ICON_WIDTH * 2,
                    SPECIAL_BUTTONS_ICON_HEIGHT,
                    SPECIAL_BUTTONS_ICON_WIDTH,
                    SPECIAL_BUTTONS_ICON_HEIGHT)));
        _waitButton.OnClick += WaitButton_OnClick;

        _hoverController = new HoverController<CombatMovementButton>();
        _hoverController.Hover += (_, e) =>
        {
            if (e is null)
            {
                return;
            }

            if (_combatant is null)
            {
                // Control is not ready to work.
                return;
            }

            var currentActorResolveValue = _combatant.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.Resolve)).Value;
            _activeCombatMovementHint = new CombatMovementHint(e.Entity, currentActorResolveValue, _combatMovementVisualizer);
            CombatMovementHover?.Invoke(this, new CombatMovementPickedEventArgs(e.Entity));
        };
        
        _hoverController.Leave += (_, e) =>
        {
            _activeCombatMovementHint = null;
            if (e is not null)
            {
                CombatMovementLeave?.Invoke(this, new CombatMovementPickedEventArgs(e.Entity));
            }
        };
    }

    public ICombatant? Combatant
    {
        get => _combatant;
        set
        {
            _combatant = value;
            _hoverController.ForcedDrop();

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

        if (!_buttons.Any(x => x is not null))
        {
            DrawTacticalPotentialExhaustedIndicator(spriteBatch: spriteBatch, contentRect: contentRect);

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
                if (_burningCombatMovement.HandSlotIndex != buttonIndex)
                {
                    continue;
                }

                _burningCombatMovement.Rect = GetButtonRectangle(buttonsRect, buttonIndex, true);
                _burningCombatMovement.Draw(spriteBatch);
            }
        }

        _waitButton.Rect = new Rectangle(
            buttonsRect.Right,
            buttonsRect.Top,
            SPECIAL_BUTTONS_ICON_WIDTH + CONTENT_MARGIN * 2,
            SPECIAL_BUTTONS_ICON_HEIGHT + CONTENT_MARGIN * 2);

        _waitButton.Draw(spriteBatch);

        if (_hoverController.CurrentValue is not null && _activeCombatMovementHint is not null)
        {
            DrawHoverInfo(_hoverController.CurrentValue, _activeCombatMovementHint, spriteBatch);
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

        _activeCombatMovementHint = null;
    }

    internal void Update(GameTime gameTime, IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        if (!IsEnabled)
        {
            return;
        }

        if (!Readonly)
        {
            KeyboardInputUpdate();

            foreach (var button in _buttons)
            {
                if (button is not null)
                {
                    button.Update(resolutionIndependentRenderer);

                    button.IsEnabled = Combatant is not null && IsResolveStatEnough(button.Entity,
                        Combatant.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.Resolve)));
                }
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

        _counterTacticalPotentialExhaustedIndicator += gameTime.ElapsedGameTime.TotalSeconds * 10;

        _waitButton.Update(resolutionIndependentRenderer);
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

    private void CombatMovementButton_OnHover(object? sender, EventArgs e)
    {
        _hoverController.HandleHover(sender as CombatMovementButton);
    }

    private void CombatMovementButton_OnLeave(object? sender, EventArgs e)
    {
        _hoverController.HandleLeave(sender as CombatMovementButton);
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

    private void DrawTacticalPotentialExhaustedIndicator(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        // Tactical potential exhausted
        // This hero can no longer act in the current battle

        var indicatorFont = _uiContentStorage.GetCombatIndicatorFont();

        var text = UiResource.TacticalPotentialExhaustedIndicator;

        var textLines = text.Split(new[]
            {
                '\n'
            },
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var colorT = (float)Math.Clamp(Math.Sin(_counterTacticalPotentialExhaustedIndicator), 0, 1);

        for (var lineIndex = 0; lineIndex < textLines.Length; lineIndex++)
        {
            var textLine = textLines[lineIndex];
            var lineSize = indicatorFont.MeasureString(textLine);

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    spriteBatch.DrawString(indicatorFont, textLine,
                        new Vector2(contentRect.Center.X - lineSize.X / 2 + i, contentRect.Y + lineIndex * 20 + j),
                        Color.Lerp(MythlandersColors.MaxDark, Color.Transparent, colorT));
                }
            }

            spriteBatch.DrawString(indicatorFont, textLine,
                new Vector2(contentRect.Center.X - lineSize.X / 2, contentRect.Y + lineIndex * 20),
                Color.Lerp(MythlandersColors.MainSciFi, Color.Transparent, colorT));
        }
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

    private static bool IsResolveStatEnough(CombatMovementInstance combatMovement,
        ICombatantStat currentCombatantResolveStat)
    {
        return combatMovement.Cost.Amount.Current <= currentCombatantResolveStat.Value.Current;
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

        button?.Click();
    }

    private void RecreateButtons()
    {
        if (_combatant is null)
        {
            return;
            //throw new InvalidOperationException("Unit required to be initialized before.");
        }

        var hand = _combatant.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand)
            .GetItems().ToArray();
        for (var buttonIndex = 0; buttonIndex < hand.Length; buttonIndex++)
        {
            var movement = hand[buttonIndex];
            if (movement is not null)
            {
                var icon = _combatMovementVisualizer.GetMoveIcon(movement.SourceMovement.Sid);
                var iconRect = UnsortedHelpers.GetIconRect(icon);
                var iconData = new IconData(_uiContentStorage.GetCombatMoveIconsTexture(), iconRect);
                var button = new CombatMovementButton(iconData, movement);
                _buttons[buttonIndex] = button;
                button.OnClick += CombatMovementButton_OnClick;
                button.OnHover += CombatMovementButton_OnHover;
                button.OnLeave += CombatMovementButton_OnLeave;
            }
            else
            {
                _buttons[buttonIndex] = null;
            }
        }
    }

    private void WaitButton_OnClick(object? sender, EventArgs e)
    {
        WaitPicked?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<CombatMovementPickedEventArgs>? CombatMovementPicked;
    public event EventHandler? WaitPicked;

    public event EventHandler<CombatMovementPickedEventArgs>? CombatMovementHover;
    public event EventHandler<CombatMovementPickedEventArgs>? CombatMovementLeave;
}