using System;

using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                _maneuverButtons[columnIndex, lineIndex] = new ManeuverButton(new FieldCoords(columnIndex, lineIndex));
                _maneuverButtons[columnIndex, lineIndex].OnClick += ManeuverButton_OnClick;
            }
        }
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
    }

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

        _isManeuversAvailable = _context.ManeuversAvailable > 0;

        for (var columnIndex = 0; columnIndex < _heroFieldSide.ColumnCount; columnIndex++)
        {
            for (var lineIndex = 0; lineIndex < _heroFieldSide.LineCount; lineIndex++)
            {
                var maneuverButton = _maneuverButtons[columnIndex, lineIndex];

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

    public event EventHandler<ManeuverSelectedEventArgs>? ManeuverSelected;

    private bool IsCoordsAvailable(FieldCoords currentCoords, FieldCoords testCoords)
    {
        return (currentCoords.ColumentIndex == testCoords.ColumentIndex &&
                Math.Abs(currentCoords.LineIndex - testCoords.LineIndex) == 1) ||
               (Math.Abs(currentCoords.ColumentIndex - testCoords.ColumentIndex) == 1 &&
                currentCoords.LineIndex == testCoords.LineIndex);
    }
}
