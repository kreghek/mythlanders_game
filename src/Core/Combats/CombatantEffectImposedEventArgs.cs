namespace Core.Combats;

public sealed class CombatantEffectImposedEventArgs : CombatantEventArgsBase
{
    public ICombatantEffect CombatantEffect { get; }

    public CombatantEffectImposedEventArgs(Combatant combatant, ICombatantEffect effect) :
        base(combatant)
    {
        CombatantEffect = effect;
    }
}