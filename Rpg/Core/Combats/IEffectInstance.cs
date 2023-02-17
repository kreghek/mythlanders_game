namespace Core.Combats;

public interface IEffectInstance
{
    void Influence(Combatant target, IEffectCombatContext context);
    void AddModifier(IUnitStatModifier modifier);
    void RemoveModifier(IUnitStatModifier modifier);
    ITargetSelector Selector { get; }
}