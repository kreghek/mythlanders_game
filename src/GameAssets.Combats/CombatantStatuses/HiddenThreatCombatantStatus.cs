using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class HiddenThreatCombatantStatus : CombatantStatusBase
{
    private HiddenThreatCombatantStatusData? _data;

    public HiddenThreatCombatantStatus(ICombatantStatusSid sid,
        ICombatantStatusLifetime lifetime, ICombatantStatusSource source) : base(sid, lifetime, source)
    {
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        _data = new HiddenThreatCombatantStatusData(combatant,
            new HiddenThreatStatModifier(new CombatantStatusModifierSource(this)), combatantEffectImposeContext.Combat);
        
        GetTargetStatValue(combatant).AddModifier(_data.Modifier);

        _data.Combat.CombatantHasBeenDamaged += Combat_CombatantHasBeenDamaged;
    }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        if (_data is not null)
        {
            _data.Combat.CombatantHasBeenDamaged -= Combat_CombatantHasBeenDamaged;
            
            GetTargetStatValue(_data.Owner).RemoveModifier(_data.Modifier);
        }
        else
        {
            //TODO Handle error
            // _data must be assigned on impose
        }
    }

    private static IStatValue GetTargetStatValue(ICombatant combatant)
    {
        return combatant.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.ShieldPoints)).Value;
    }

    private void Combat_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        if (_data is null)
        {
            //TODO Handle error
            // _data must be assigned on impose
        }
        else
        {
            if (_data.Owner != e.Combatant)
            {
                _data.Modifier.IncrementValue();
                GetTargetStatValue(_data.Owner).Restore(1);
            }
            else
            {
                _data.Modifier.ClearValue();
            }
        }
    }
}