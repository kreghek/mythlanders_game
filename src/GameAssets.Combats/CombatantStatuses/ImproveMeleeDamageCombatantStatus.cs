using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using GameAssets.Combats.CombatantStatusLifetimes;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class ImproveMeleeDamageCombatantStatus : CombatantStatusBase
{
    private readonly IStatModifier _statModifier;

    public ImproveMeleeDamageCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
        ICombatantStatusSource source, int value) :
        base(sid, lifetime, source)
    {
        Value = value;
        _statModifier = new StatModifier(Value, Singleton<NullStatModifierSource>.Instance);
    }

    public int Value { get; }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        foreach (var combatMovementContainer in combatant.CombatMovementContainers)
        {
            foreach (var combatMovementInstance in combatMovementContainer.GetItems())
            {
                if (combatMovementInstance is not null)
                {
                    foreach (var effectInstance in combatMovementInstance.Effects)
                    {
                        if (effectInstance is DamageEffectInstance)
                        {
                            effectInstance.RemoveModifier(_statModifier);
                        }
                    }
                }
            }
        }
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        base.Impose(combatant, context);

        foreach (var combatMovementContainer in combatant.CombatMovementContainers)
        {
            foreach (var combatMovementInstance in combatMovementContainer.GetItems())
            {
                if (combatMovementInstance is not null && IsCombatMovementMelee(combatMovementInstance.SourceMovement))
                {
                    foreach (var effectInstance in combatMovementInstance.Effects)
                    {
                        if (effectInstance is DamageEffectInstance)
                        {
                            effectInstance.AddModifier(_statModifier);
                        }
                    }
                }
            }
        }
    }

    private static bool IsCombatMovementMelee(CombatMovement combatMovement)
    {
        if (combatMovement.Metadata is CombatMovementMetadata metadata)
        {
            return metadata.Traits.Contains(CombatMovementMetadataTraits.Melee);
        }

        return false;
    }
}

public sealed class ReduceEnemiesShieldPointsCombatantStatus : CombatantStatusBase
{
    private readonly IStatModifier _statModifier;

    public ReduceEnemiesShieldPointsCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
        ICombatantStatusSource source, int value) :
        base(sid, lifetime, source)
    {
        Value = value;
        _statModifier = new StatModifier(Value, Singleton<NullStatModifierSource>.Instance);
    }

    public int Value { get; }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        if (_combat is not null)
        {
            _combat.CombatantHasBeenAdded -= Combat_CombatantHasBeenAdded;
        }
        else
        {
#if DEBUG
            throw new InvalidOperationException("Combat engine was not assigned in Impose method");
#endif
        }
    }

    private ICombatant? _owner;
    private CombatEngineBase? _combat;
    
    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        base.Impose(combatant, context);

        _owner = combatant;
        _combat = context.Combat;

        var enemies = context.Combat.CurrentCombatants.Where(x => x.IsPlayerControlled != combatant.IsPlayerControlled);

        // Add status to current combatants
        foreach (var enemy in enemies)
        {
            AddReduceStatus(enemy, combatant, context.Combat);
        }
        
        context.Combat.CombatantHasBeenAdded += Combat_CombatantHasBeenAdded; 
    }

    private void Combat_CombatantHasBeenAdded(object? sender, CombatantHasBeenAddedEventArgs e)
    {
        var combatant = (ICombatant?)sender;
        
        if (_combat is not null)
        {
            if (combatant is null || _owner is null)
            {
#if DEBUG
                throw new InvalidOperationException("Status owner was not assigned in Impose method or new combatant is null.");
#endif
            }

            if (combatant.IsPlayerControlled == _owner.IsPlayerControlled)
            {
                // Do not add status new allies
                return;
            }

            // Add status to new combatant
                    
            AddReduceStatus(combatant,  combatant, _combat);

        }
        else
        {
#if DEBUG
            throw new InvalidOperationException("Combat engine was not assigned in Impose method");
#endif
        }
    }

    private void AddReduceStatus(ICombatant enemy, ICombatant combatant, CombatEngineBase combat)
    {
        enemy.AddStatus(new ModifyStatCombatantStatus(new CombatantStatusSid(Sid + "_Target"),
                new TargetCombatantBoundCombatantStatusLifetime(combatant), Source, CombatantStatTypes.ShieldPoints,
                _statModifier.Value), new CombatantStatusImposeContext(combat),
            new CombatantStatusLifetimeImposeContext(combatant, combat));
    }
}
