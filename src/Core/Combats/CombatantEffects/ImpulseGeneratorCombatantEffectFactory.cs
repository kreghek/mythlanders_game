namespace Core.Combats.CombatantEffects;

public sealed class ImpulseGeneratorCombatantEffectFactory : ICombatantEffectFactory
{
    private readonly ICombatantEffectSid _generatedSid;
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly ICombatantEffectSid _sid;

    public ImpulseGeneratorCombatantEffectFactory(ICombatantEffectSid sid, ICombatantEffectSid generatedSid,
        ICombatantEffectLifetimeFactory lifetimeFactory)
    {
        _sid = sid;
        _generatedSid = generatedSid;
        _lifetimeFactory = lifetimeFactory;
    }

    public ICombatantEffect Create()
    {
        return new ImpulseGeneratorCombatantEffect(_sid, _generatedSid, _lifetimeFactory.Create());
    }
}