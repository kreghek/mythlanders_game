using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats.TargetSelectors;

namespace Core.Combats.CombatantStatuses;

public sealed class ConterAttackCombatantStatus : CombatantStatusBase
{

    private readonly CombatMovement _combatMovement;
    private CombatEngineBase? _combat;
    private ICombatant? _ownerCombatant;

    public ConterAttackCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
        
    }

    public override void Dispel(ICombatant combatant)
    {
        _combat!.CombatantHasBeenDamaged -= Combat_CombatantHasBeenDamaged;
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        _combat = context.Combat;
        _ownerCombatant = combatant;

        _combat.CombatantHasBeenDamaged += Combat_CombatantHasBeenDamaged;
    }

    private void Combat_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        if (e.Combatant != _ownerCombatant)
        {
            return;
        }

        e.Combatant.Behaviour.HandleIntention()
        
        var combat = (CombatEngineBase)sender!;


        var execution = combat.CreateCombatMovementExecution(new CombatMovementInstance(_combatMovement));

    }
}
