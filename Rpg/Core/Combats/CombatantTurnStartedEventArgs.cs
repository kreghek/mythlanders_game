namespace Core.Combats;

public sealed class CombatantTurnStartedEventArgs : CombatantEventArgsBase
{
    public CombatantTurnStartedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}
