namespace Core.Combats.CombatantStatus;

public sealed class ModifyEffectsCombatantStatusFactory : ICombatantStatusFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly ICombatantStatusSid _sid;
    private readonly int _value;

    public ModifyEffectsCombatantStatusFactory(ICombatantStatusSid sid, ICombatantEffectLifetimeFactory lifetimeFactory,
        int value)
    {
        _sid = sid;
        _lifetimeFactory = lifetimeFactory;
        _value = value;
    }

    public ICombatantStatus Create()
    {
        return new ModifyEffectsCombatantStatus(_sid, _lifetimeFactory.Create(), _value);
    }
}