namespace Core.Combats;

public sealed class CombatantHandChangedEventArgs : CombatantEventArgsBase
{
    public CombatantHandChangedEventArgs(Combatant combatant, CombatMovementInstance move, int handSlotIndex) : base(combatant)
    {
        Move = move;
        HandSlotIndex = handSlotIndex;
    }

    public CombatMovementInstance Move { get; }
    public int HandSlotIndex { get; }
}