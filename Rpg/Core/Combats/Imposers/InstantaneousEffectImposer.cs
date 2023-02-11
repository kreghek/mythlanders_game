namespace Core.Combats.Imposers;

public sealed class InstantaneousEffectImposer : IEffectImposer
{
    public void Impose(IEffect effect, Combatant target, IEffectCombatContext context)
    {
        effect.Influence(target, context);
    }

    public void Update(EffectImposerUpdateType updateType)
    {
        // Do nothing
    }
}