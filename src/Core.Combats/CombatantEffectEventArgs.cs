namespace Core.Combats;

/// <summary>
/// Event argumanets to pass combatant effect in a effect's events.
/// </summary>
public sealed class CombatantEffectEventArgs : CombatantEventArgsBase
{
    public CombatantEffectEventArgs(ICombatant combatant, ICombatantStatus effect) :
        base(combatant)
    {
        CombatantEffect = effect;
    }

    public ICombatantStatus CombatantEffect { get; }
}