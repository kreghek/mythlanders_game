using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class OwnerStatBelowLifetimeExpirationCondition : ICombatantStateLifetimeExpirationCondition
{
    private readonly ICombatantStatType _targetType;
    private readonly int _threshold;

    public OwnerStatBelowLifetimeExpirationCondition(ICombatantStatType targetType, int threshold)
    {
        _targetType = targetType;
        _threshold = threshold;
    }

    public bool Check(ICombatant statusOwner, CombatEngineBase combatEngine)
    {
        return statusOwner.Stats.Single(x => Equals(x.Type, _targetType)).Value.Current <= _threshold;
    }
}