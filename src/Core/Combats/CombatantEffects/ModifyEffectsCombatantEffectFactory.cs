namespace Core.Combats.CombatantEffects;

public sealed class ModifyEffectsCombatantEffectFactory : ICombatantEffectFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly ICombatantEffectSid _sid;
    private readonly int _value;

    public ModifyEffectsCombatantEffectFactory(ICombatantEffectSid sid, ICombatantEffectLifetimeFactory lifetimeFactory,
        int value)
    {
        _sid = sid;
        _lifetimeFactory = lifetimeFactory;
        _value = value;
    }

    public ICombatantEffect Create()
    {
        return new ModifyEffectsCombatantEffect(_sid, _lifetimeFactory.Create(), _value);
    }
}