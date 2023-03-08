namespace Core.Combats;

public abstract class CombatantEventArgsBase : EventArgs
{
    protected CombatantEventArgsBase(Combatant combatant)
    {
        Combatant = combatant;
    }

    public Combatant Combatant { get; }
}

public sealed class CombatantDefeatedEventArgs : CombatantEventArgsBase
{
    public CombatantDefeatedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}

public sealed class CombatantEndsTurnEventArgs : CombatantEventArgsBase
{
    public CombatantEndsTurnEventArgs(Combatant combatant) : base(combatant)
    {
    }
}

public sealed class CombatantTurnStartedEventArgs : CombatantEventArgsBase
{
    public CombatantTurnStartedEventArgs(Combatant combatant) : base(combatant)
    {
    }
}