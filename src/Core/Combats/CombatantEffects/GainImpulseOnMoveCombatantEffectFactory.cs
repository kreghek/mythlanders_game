namespace Core.Combats.CombatantEffects;

public sealed class GainImpulseOnMoveCombatantEffectFactory : ICombatantEffectFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;

    public GainImpulseOnMoveCombatantEffectFactory(ICombatantEffectLifetimeFactory lifetimeFactory)
    {
        _lifetimeFactory = lifetimeFactory;
    }

    public ICombatantEffect Create()
    {
        return new ImpulseGeneratorCombatantEffect(_lifetimeFactory.Create());
    }
}