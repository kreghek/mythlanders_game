using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class HiddenThreatCombatantStatus : CombatantStatusBase
{
    private ICombatant? _statusOwner;

    private HiddenThreatStatModifier? _modifier;
    private CombatEngineBase? _combat;

    public HiddenThreatCombatantStatus(ICombatantStatusSid sid,
        ICombatantStatusLifetime lifetime, ICombatantStatusSource source) : base(sid, lifetime, source)
    {
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        _statusOwner = combatant;
        _modifier = new HiddenThreatStatModifier(new CombatantStatusModifierSource(this));
        _statusOwner.Stats.Single(x=>ReferenceEquals(x.Type, CombatantStatTypes.ShieldPoints)).Value.AddModifier(_modifier);

        _combat = combatantEffectImposeContext.Combat;
        
        _combat.CombatantHasBeenDamaged += Combat_CombatantHasBeenDamaged;
    }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        if (_combat is not null)
        {
            _combat.CombatantHasBeenDamaged -= Combat_CombatantHasBeenDamaged;
        }
        else
        {
            //TODO Handle error
            // combat must be assigned on impose
        }

        if (_modifier is not null && _statusOwner is not null)
        {
            _statusOwner.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.ShieldPoints)).Value
                .RemoveModifier(_modifier);
        }
        else
        {
            //TODO Handle error
        }
    }

    private void Combat_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        if (_statusOwner != e.Combatant)
        {
            if (_modifier is not null)
            {
                _modifier.IncrementValue();
            }
            else
            {
                //TODO Handle error
                // modifier must be created on impose
            }
        }
    }
}