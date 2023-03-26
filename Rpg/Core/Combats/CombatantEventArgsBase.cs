namespace Core.Combats;

public abstract class CombatantEventArgsBase : EventArgs
{
    protected CombatantEventArgsBase(Combatant combatant)
    {
        Combatant = combatant;
    }

    public Combatant Combatant { get; }
}