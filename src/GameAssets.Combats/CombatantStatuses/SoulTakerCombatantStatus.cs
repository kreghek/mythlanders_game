using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

namespace Core.Combats.CombatantStatuses;

public sealed class SoulTakerCombatantStatus : CombatantStatusBase
{
    private readonly ICombatantStatusSid _generatedSid;
    private ICombatant? _statusOwner;

    public SoulTakerCombatantStatus(ICombatantStatusSid sid, ICombatantStatusSid generatedSid,
        ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
        _generatedSid = generatedSid;
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        _statusOwner = combatant;

        combatantEffectImposeContext.Combat.CombatantHasBeenDamaged += Combat_CombatantHasBeenDamaged;
    }

    private void Combat_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        var shieldStatus = new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(_generatedSid, Lifetime, CombatantStatTypes.ShieldPoints, 1));

        var targetCombat = (CombatEngineBase)sender!;

        targetCombat.ImposeCombatantStatus(_statusOwner!, shieldStatus);
    }
}