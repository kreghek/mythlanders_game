using System;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame;

namespace Client.GameScreens.Combat.Ui;

internal class FieldManeuversVisualizer
{
    private const int BUTTON_SIZE = 40;

    private readonly ICombatantPositionProvider _combatantPositionProvider;
    private readonly IManeuverContext _context;
    private readonly CombatFieldSide _heroFieldSide;

    private readonly HoverController<ManeuverButton> _hoverController;
    private readonly Matrix<ManeuverButton> _maneuverButtons;
    private readonly SpriteFont _spriteFont;

    private float _animationCounter;
    private ICombatant? _combatant;

    private bool _isManeuversAvailable;

    public FieldManeuversVisualizer(ICombatantPositionProvider combatantPositionProvider, IManeuverContext context,
        SpriteFont spriteFont)
    {
        _combatantPositionProvider = combatantPositionProvider;
        _context = context;
        _spriteFont = spriteFont;
        _heroFieldSide = context.FieldSide;

        _maneuverButtons =
            new Matrix<ManeuverButton>(_heroFieldSide.ColumnCount, _heroFieldSide.LineCount);

        for (var columnIndex = 0; columnIndex < _heroFieldSide.ColumnCount; columnIndex++)
        {
            for (var lineIndex = 0; lineIndex < _heroFieldSide.LineCount; lineIndex++)
            {
                var maneuverButton = new ManeuverButton(new FieldCoords(columnIndex, lineIndex));

                maneuverButton.OnClick += ManeuverButton_OnClick;
                maneuverButton.OnHover += ManeuverButton_OnHover;
                maneuverButton.OnLeave += ManeuverButton_OnLeave;

                _maneuverButtons[columnIndex, lineIndex] = maneuverButton;
            }
        }

        _hoverController = new HoverController<ManeuverButton>();
        _hoverController.Hover += (s, e) => { Hover?.Invoke(this, EventArgs.Empty); };
        _hoverController.Leave += (s, e) => { Leave?.Invoke(this, EventArgs.Empty); };
    }

    /// <summary>
    /// Current combatant for which maneuver controls draw.
    /// </summary>
    public ICombatant? Combatant
    {
        get => _combatant;
        set
        {
            _combatant = value;
            _hoverController.ForcedDrop();
        }
    }

    public bool IsHidden { get; internal set; }

    /// <summary>
    /// Draw maneuver controls on the combat field.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch)
    {
        if (Combatant is null)
        {
            return;
        }

        if (!_isManeuversAvailable)
        {
            return;
        }

        for (var columnIndex = 0; columnIndex < _heroFieldSide.ColumnCount; columnIndex++)
        {
            for (var lineIndex = 0; lineIndex < _heroFieldSide.LineCount; lineIndex++)
            {
                var maneuverButton = _maneuverButtons[columnIndex, lineIndex];

                if (maneuverButton.IsEnabled)
                {
                    var position =
                        _combatantPositionProvider.GetPosition(maneuverButton.FieldCoords,
                            CombatantPositionSide.Heroes);

                    maneuverButton.Rect = new Rectangle(
                        (int)position.X - BUTTON_SIZE / 2,
                        (int)position.Y - BUTTON_SIZE / 2,
                        BUTTON_SIZE,
                        BUTTON_SIZE);
                    maneuverButton.Draw(spriteBatch);
                }
                else
                {
                    var position =
                        _combatantPositionProvider.GetPosition(maneuverButton.FieldCoords,
                            CombatantPositionSide.Heroes);

                    maneuverButton.Rect = new Rectangle(
                        (int)position.X - BUTTON_SIZE / 2,
                        (int)position.Y - BUTTON_SIZE / 2,
                        BUTTON_SIZE,
                        BUTTON_SIZE);
                    maneuverButton.Draw(spriteBatch);
                }
            }
        }

        if (_hoverController.CurrentValue is not null && _context.ManeuverStartCoords is not null)
        {
            var arrowStart = GetPosition(_context.ManeuverStartCoords);
            var arrowTarget = GetPosition(_hoverController.CurrentValue.FieldCoords);

            var color = Color.Lerp(Color.Cyan, Color.Transparent, (float)Math.Sin(_animationCounter * 1.25) * 0.5f);
            spriteBatch.DrawLine(arrowStart, arrowTarget, color, Math.Max((float)Math.Sin(_animationCounter) * 3, 1));
            spriteBatch.DrawCircle(arrowStart, (float)Math.Sin(_animationCounter) * 5, 6, color);
            spriteBatch.DrawCircle(arrowTarget, (float)Math.Sin(_animationCounter) * 5, 6, color);
        }

        if (_hoverController.CurrentValue?.FieldCoords is not null)
        {
            var position = _combatantPositionProvider.GetPosition(_hoverController.CurrentValue.FieldCoords,
                CombatantPositionSide.Heroes);
            spriteBatch.DrawLine(position, position + Vector2.UnitX * 600,
                Color.Lerp(Color.Cyan, Color.Transparent, 0.75f), 10);
            spriteBatch.DrawLine(position - Vector2.UnitY * 60, position + Vector2.UnitY * 60,
                Color.Lerp(Color.Cyan, Color.Transparent, 0.75f), 10);
            spriteBatch.DrawString(_spriteFont,
                _hoverController.CurrentValue.FieldCoords.ColumentIndex == 0
                    ? UiResource.FieldManeuversVisualizer_Draw_Avanguard
                    : UiResource.FieldManeuversVisualizer_Draw_Rearguard,
                position - new Vector2(20, 20), Color.Lerp(Color.Cyan, Color.Transparent, 0.15f),
                MathHelper.ToRadians(-90), Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }

    /// <summary>
    /// Update state of the panel
    /// </summary>
    public void Update(IScreenProjection screenProjection)
    {
        if (Combatant is null)
        {
            return;
        }

        if (IsHidden)
        {
            return;
        }

        _animationCounter += 0.1f;

        _isManeuversAvailable = _context.ManeuversAvailable > 0;

        for (var columnIndex = 0; columnIndex < _heroFieldSide.ColumnCount; columnIndex++)
        {
            for (var lineIndex = 0; lineIndex < _heroFieldSide.LineCount; lineIndex++)
            {
                var maneuverButton = _maneuverButtons[columnIndex, lineIndex];

                if (!_isManeuversAvailable)
                {
                    continue;
                }

                if (_context.ManeuverStartCoords is not null)
                {
                    if (IsCoordsAvailable(_context.ManeuverStartCoords, maneuverButton.FieldCoords))
                    {
                        maneuverButton.IsEnabled = true;
                        maneuverButton.Update(screenProjection);
                    }
                    else
                    {
                        maneuverButton.IsEnabled = false;
                    }
                }
            }
        }
    }

    private Vector2 GetPosition(FieldCoords coords)
    {
        return _combatantPositionProvider.GetPosition(coords, CombatantPositionSide.Heroes);
    }

    private static bool IsCoordsAvailable(FieldCoords currentCoords, FieldCoords testCoords)
    {
        return (currentCoords.ColumentIndex == testCoords.ColumentIndex &&
                Math.Abs(currentCoords.LineIndex - testCoords.LineIndex) == 1) ||
               (Math.Abs(currentCoords.ColumentIndex - testCoords.ColumentIndex) == 1 &&
                currentCoords.LineIndex == testCoords.LineIndex);
    }

    private void ManeuverButton_OnClick(object? sender, EventArgs e)
    {
        if (sender is null)
        {
            throw new InvalidOperationException();
        }

        var maneuverButton = (ManeuverButton)sender;
        ManeuverSelected?.Invoke(this, new ManeuverSelectedEventArgs(maneuverButton.FieldCoords));
    }

    private void ManeuverButton_OnHover(object? sender, EventArgs e)
    {
        var maneuverButton = sender as ManeuverButton;

        _hoverController.HandleHover(maneuverButton);
    }

    private void ManeuverButton_OnLeave(object? sender, EventArgs e)
    {
        var maneuverButton = sender as ManeuverButton;

        _hoverController.HandleLeave(maneuverButton);
    }

    public event EventHandler<ManeuverSelectedEventArgs>? ManeuverSelected;
    public event EventHandler? Hover;
    public event EventHandler? Leave;
}