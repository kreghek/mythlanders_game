namespace Core.Combats;

public sealed class CombatantInterruptedEventArgs : CombatantEventArgsBase
{
    public CombatantInterruptedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}