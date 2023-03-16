using System;

using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.Ui;

internal class FieldManeuversVisualizer
{
    private const int BUTTON_SIZE = 40;

    private readonly ICombatantPositionProvider _combatantPositionProvider;
    private readonly IManeuverContext _context;
    private readonly CombatFieldSide _heroFieldSide;
    private readonly Matrix<ManeuverButton> _maneuverButtons;

    public FieldManeuversVisualizer(ICombatantPositionProvider combatantPositionProvider, IManeuverContext context)
    {
        _combatantPositionProvider = combatantPositionProvider;
        _context = context;
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
    }

    private void ManeuverButton_OnLeave(object? sender, EventArgs e)
    {
        _selectedManeuverButton = null;
    }

    private ManeuverButton? _selectedManeuverButton;

    private void ManeuverButton_OnHover(object? sender, EventArgs e)
    {
        _selectedManeuverButton = sender as ManeuverButton;
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

    /// <summary>
    /// Current combatant for which maneuver controls draw.
    /// </summary>
    public Combatant? Combatant { get; set; }

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
            }
        }

        if (_selectedManeuverButton is not null && _context.ManeuverStartCoords is not null)
        {
            var arrowStart = GetPosition(_context.ManeuverStartCoords);
            var arrowTarget = GetPosition(_selectedManeuverButton.FieldCoords);

            var color = Color.Lerp(Color.Cyan, Color.Transparent, (float)Math.Sin(_animationCounter * 1.25) * 0.5f);
            spriteBatch.DrawLine(arrowStart, arrowTarget, color, Math.Max((float)Math.Sin(_animationCounter) * 3, 1));
            spriteBatch.DrawCircle(arrowStart, (float)Math.Sin(_animationCounter) * 5, 6, color);
            spriteBatch.DrawCircle(arrowTarget, (float)Math.Sin(_animationCounter) * 5, 6, color);
        }
    }

    private Vector2 GetPosition(FieldCoords coords)
    {
        return _combatantPositionProvider.GetPosition(coords, CombatantPositionSide.Heroes);
    }

    private float _animationCounter;

    private bool _isManeuversAvailable;

    /// <summary>
    /// Update state of the panel
    /// </summary>
    public void Update(ResolutionIndependentRenderer rir)
    {
        if (Combatant is null)
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

                if (_context.ManeuverStartCoords is not null)
                {
                    if (IsCoordsAvailable(_context.ManeuverStartCoords, maneuverButton.FieldCoords))
                    {
                        maneuverButton.IsEnabled = true;
                        maneuverButton.Update(rir);
                    }
                    else
                    {
                        maneuverButton.IsEnabled = false;
                    }
                }
            }
        }
    }

    public event EventHandler<ManeuverSelectedEventArgs>? ManeuverSelected;

    private static bool IsCoordsAvailable(FieldCoords currentCoords, FieldCoords testCoords)
    {
        return (currentCoords.ColumentIndex == testCoords.ColumentIndex &&
                Math.Abs(currentCoords.LineIndex - testCoords.LineIndex) == 1) ||
               (Math.Abs(currentCoords.ColumentIndex - testCoords.ColumentIndex) == 1 &&
                currentCoords.LineIndex == testCoords.LineIndex);
    }
}