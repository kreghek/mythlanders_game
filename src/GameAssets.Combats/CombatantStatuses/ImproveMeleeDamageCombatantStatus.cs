using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

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
