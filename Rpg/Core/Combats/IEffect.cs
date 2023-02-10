namespace Core.Combats;

public interface IEffect
{
    ITargetSelector Selector { get; }
    
    IEffectImposer Imposer { get; }

    void Influence(Combatant target, IEffectCombatContext context);
}