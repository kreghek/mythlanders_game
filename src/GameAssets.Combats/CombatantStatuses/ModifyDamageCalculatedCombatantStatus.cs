using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace GameAssets.Combats.CombatantStatuses;

/// <summary>
/// Change combat movements damage based on stat value.
/// </summary>
public sealed class ModifyDamageCalculatedCombatantStatus : CombatantStatusBase
{
    private readonly Func<ICombatant, int> _valueDelegate;
    private IStatModifier? _statModifier;

    public ModifyDamageCalculatedCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime, ICombatantStatusSource source, Func<ICombatant, int> valueDelegate) :
        base(sid, lifetime, source)
    {
        _valueDelegate = valueDelegate;
    }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        if (_statModifier is null)
        {
            //TODO log error
            // It is error because stat modifier created in Impose.
            // But status can't be dispelled before it imposed.  
            return;
        }

        foreach (var combatMovementContainer in combatant.CombatMovementContainers)
        {
            foreach (var combatMovementInstance in combatMovementContainer.GetItems())
            {
                if (combatMovementInstance is null)
                {
                    continue;
                }

                foreach (var effectInstance in combatMovementInstance.Effects)
                {
                    effectInstance.RemoveModifier(_statModifier);
                }
            }
        }
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        base.Impose(combatant, context);

        _statModifier = new StatModifier(_valueDelegate(combatant), new NullStatModifierSource());

        foreach (var combatMovementContainer in combatant.CombatMovementContainers)
        {
            foreach (var combatMovementInstance in combatMovementContainer.GetItems())
            {
                if (combatMovementInstance is not null)
                {
                    foreach (var effectInstance in combatMovementInstance.Effects)
                    {
                        effectInstance.AddModifier(_statModifier);
                    }
                }
            }
        }
    }
}