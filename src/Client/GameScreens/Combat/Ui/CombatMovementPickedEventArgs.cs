using System;

using CombatDicesTeam.Combats;

namespace Client.GameScreens.Combat.Ui;

public sealed class CombatMovementPickedEventArgs : EventArgs
{
    public CombatMovementPickedEventArgs(CombatMovementInstance combatMovement)
    {
        CombatMovement = combatMovement;
    }

    public CombatMovementInstance CombatMovement { get; }
}