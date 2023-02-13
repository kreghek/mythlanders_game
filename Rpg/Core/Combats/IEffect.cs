namespace Core.Combats;

public interface IEffect
{
    IEffectImposer Imposer { get; }
    ITargetSelector Selector { get; }

    void Influence(Combatant target, IEffectCombatContext context);
}