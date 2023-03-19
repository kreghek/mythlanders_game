namespace Core.Combats;

public sealed class CombatantDefeatedEventArgs : CombatantEventArgsBase
{
    public CombatantDefeatedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}