using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats.TargetSelectors;

namespace Core.Combats.CombatantStatuses;

public sealed class ConterAttackCombatantStatus : CombatantStatusBase
{
    private readonly ITargetSelector _conterAttackTargetSelector;

    public ConterAttackCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
        _conterAttackTargetSelector = new AttackerTargetSelector();
    }

    public override void Dispel(ICombatant combatant)
    {
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        context.Combat.CombatantHasBeenDamaged += Combat_CombatantHasBeenDamaged;
    }

    private void Combat_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        
    }
}
