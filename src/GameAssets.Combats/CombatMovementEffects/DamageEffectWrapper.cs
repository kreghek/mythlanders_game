using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

namespace GameAssets.Combats.CombatMovementEffects;

public class DamageEffectWrapper : IEffect
{
    private readonly DamageEffect _innerDamageEffect;

    public DamageEffectWrapper(ITargetSelector selector, DamageType damageType, GenericRange<int> damage)
    {
        _innerDamageEffect = new DamageEffect(selector, damageType, damage,
            new DamageEffectConfig(CombatantStatTypes.HitPoints, CombatantStatTypes.ShieldPoints,
                CombatantStatTypes.Defense));
    }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => _innerDamageEffect.ImposeConditions;

    public ITargetSelector Selector => _innerDamageEffect.Selector;

    public IEffectInstance CreateInstance()
    {
        return _innerDamageEffect.CreateInstance();
    }

    public GenericRange<int> Damage => _innerDamageEffect.Damage;
}