namespace Core.Combats;

public sealed class InstantaneousEffectImposer : IEffectImposer
{
    public void Impose(IEffect effect, Combatant target, IEffectCombatContext context)
    {
        effect.Influence(target, context);
    }
}