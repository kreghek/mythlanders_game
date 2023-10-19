using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.CombatantStatuses;
public sealed class AutoRestoreChangeStatCombatantStatus : ICombatantStatus
{
    private readonly ChangeStatCombatantStatus _baseStatus;

    public AutoRestoreChangeStatCombatantStatus(ChangeStatCombatantStatus baseStatus)
    {
        _baseStatus = baseStatus;
    }

    public ICombatantStatusLifetime Lifetime => _baseStatus.Lifetime;
    public ICombatantStatusSid Sid => _baseStatus.Sid;

    public void Dispel(ICombatant combatant)
    {
        _baseStatus.Dispel(combatant);
    }

    public void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        _baseStatus.Impose(combatant, combatantEffectImposeContext);

        var targetStat = combatant.Stats.Single(x => x.Type == _baseStatus.StatType);
        targetStat.Value.Restore();
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        _baseStatus.Update(updateType, context);
    }
}
