namespace Core.Combats;

public interface ICombatantEffect
{
    void Impose(Combatant combatant);
    void Dispel(Combatant combatant);
    void Update(CombatantEffectUpdateType updateType);
    ICombatantEffectLifetime Lifetime { get; }
}