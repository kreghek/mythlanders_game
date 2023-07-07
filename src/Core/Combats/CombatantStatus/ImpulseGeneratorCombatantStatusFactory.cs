namespace Core.Combats.CombatantStatus;

public sealed class ImpulseGeneratorCombatantStatusFactory : ICombatantStatusFactory
{
    private readonly ICombatantStatusSid _generatedSid;
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly ICombatantStatusSid _sid;

    public ImpulseGeneratorCombatantStatusFactory(ICombatantStatusSid sid, ICombatantStatusSid generatedSid,
        ICombatantEffectLifetimeFactory lifetimeFactory)
    {
        _sid = sid;
        _generatedSid = generatedSid;
        _lifetimeFactory = lifetimeFactory;
    }

    public ICombatantStatus Create()
    {
        return new ImpulseGeneratorCombatantStatus(_sid, _generatedSid, _lifetimeFactory.Create());
    }
}