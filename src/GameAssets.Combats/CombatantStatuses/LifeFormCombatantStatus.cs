using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class LifeFormCombatantStatus: SystemCombatantStatus
{
    public LifeFormCombatantStatus(LifeForm lifeForm)
    {
        Sid = new CombatantStatusSid(lifeForm.ToString());
    }

    public override ICombatantStatusLifetime Lifetime { get; } = new OwnerBoundCombatantEffectLifetime();
    public override ICombatantStatusSid Sid { get; }
    public override ICombatantStatusSource Source { get; } = Singleton<NullCombatantStatusSource>.Instance;
}