namespace Core.Combats.CombatantEffects;

public sealed class ModifyEffectsCombatantEffectFactory : ICombatantEffectFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly int _value;

    public ModifyEffectsCombatantEffectFactory(ICombatantEffectLifetimeFactory lifetimeFactory, int value)
    {
        _lifetimeFactory = lifetimeFactory;
        this._value = value;
    }

    public ICombatantEffect Create()
    {
        return new ModifyEffectsCombatantEffect(_lifetimeFactory.Create(), _value);
    }
}