using Core.Combats;

namespace Client.GameScreens.Combat.Ui;

internal interface ISkillPanelState
{
    public CombatMovementInstance? SelectedCombatMovement { get; }
}