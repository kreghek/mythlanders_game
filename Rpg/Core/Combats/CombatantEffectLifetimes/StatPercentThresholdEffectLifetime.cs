namespace Core.Combats.CombatantEffectLifetimes;

internal class StatPercentThresholdEffectLifetime : ICombatantEffectLifetime
{
    private readonly float _minShare;
    private readonly UnitStatType _statType;

    public StatPercentThresholdEffectLifetime(UnitStatType statType, float minShare)
    {
        _statType = statType;
        _minShare = minShare;
    }

    public bool IsDead { get; private set; }

    public void Update(CombatantEffectUpdateType updateType, ICombatantEffectLifetimeUpdateContext context)
    {
        if (updateType == CombatantEffectUpdateType.EndRound)
        {
            var stat = context.Combatant.Stats.SingleOrDefault(x => x.Type == _statType);

            if (stat is null)
            {
                IsDead = true;
                return;
            }

            if (stat.Value.GetShare() < _minShare) IsDead = true;
        }
    }

    public void EffectImposed(ICombatantEffect combatantEffect, ICombatantEffectLifetimeImposeContext context)
    {
        
    }

    public void EffectDispelled(ICombatantEffect combatantEffect, ICombatantEffectLifetimeDispelContext context)
    {
        
    }
}