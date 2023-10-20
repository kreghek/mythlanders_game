using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class DefensiveStanceCombatantStatusWrapper : ICombatantStatus
{
    private readonly ICombatantStatus _baseStatus;

    public DefensiveStanceCombatantStatusWrapper(ICombatantStatus baseStatus)
    {
        _baseStatus = baseStatus;
    }

    public void Dispel(ICombatant combatant)
    {
        _baseStatus.Dispel(combatant);
    }

    public void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        _baseStatus.Impose(combatant, combatantEffectImposeContext);
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        _baseStatus.Update(updateType, context);
    }

    public ICombatantStatusLifetime Lifetime => _baseStatus.Lifetime;
    public ICombatantStatusSid Sid => _baseStatus.Sid;
}