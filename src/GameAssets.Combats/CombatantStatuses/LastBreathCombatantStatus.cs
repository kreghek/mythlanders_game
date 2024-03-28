using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.CombatantStatuses;

public class LastBreathCombatantStatus: CombatantStatusBase
{
    private readonly int _thresholdValue;
    private readonly int _restoreValue;
    private ICombatant? _owner;
    private CombatEngineBase? _combat;

    public LastBreathCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime, ICombatantStatusSource source, int thresholdValue, int restoreValue) : base(sid, lifetime, source)
    {
        _thresholdValue = thresholdValue;
        _restoreValue = restoreValue;
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        base.Impose(combatant, context);

        _owner = combatant;
        _combat = context.Combat;
        
        _combat.CombatantHasBeenDamaged += Combat_CombatantHasBeenDamaged;
    }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        if (_combat is not null)
        {
            _combat.CombatantHasBeenDamaged -= Combat_CombatantHasBeenDamaged;
        }
    }

    private void Combat_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        if (_owner is not null)
        {
            if (e.Combatant == _owner)
            {
                var targetStat = e.Combatant.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.HitPoints));
                if (targetStat.Value.Current <= _thresholdValue && targetStat.Value.Current > 0)
                {
                    targetStat.Value.Restore(_restoreValue);
                }
            }
        }
    }
}