namespace Core.Combats;

public sealed class CombatantEndsTurnEventArgs : CombatantEventArgsBase
{
    public CombatantEndsTurnEventArgs(Combatant combatant) : base(combatant)
    {
    }
}