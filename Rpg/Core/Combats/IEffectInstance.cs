namespace Core.Combats;

public interface IEffectInstance
{
    ITargetSelector Selector { get; }
    void AddModifier(IUnitStatModifier modifier);
    void Influence(Combatant target, IEffectCombatContext context);
    void RemoveModifier(IUnitStatModifier modifier);
}