namespace Core.Combats;

public sealed class CombatantDefeatedEventArgs : CombatantEventArgsBase
{
    public CombatantDefeatedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}

public sealed class CombatantInterruptedEventArgs : CombatantEventArgsBase
{
    public CombatantInterruptedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}