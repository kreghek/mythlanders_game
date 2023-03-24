namespace Core.Combats.CombatantEffects;

public sealed class ModifyCombatantMoveStatsCombatantEffect:CombatantEffectBase
{
    private readonly CombatantMoveStats _stats;
    private readonly StatModifier _modifier;

    public ModifyCombatantMoveStatsCombatantEffect(ICombatantEffectLifetime lifetime, CombatantMoveStats stats, int value) : base(lifetime)
    {
        _stats = stats;

        _modifier = new StatModifier(value);
    }

    public override void Impose(Combatant combatant)
    {
        base.Impose(combatant);

        foreach (var combatMovementInstance in combatant.Hand)
        {
            if (combatMovementInstance is not null)
            {
                switch (_stats)
                {
                    case CombatantMoveStats.Cost:
                        combatMovementInstance.Cost.Amount.AddModifier(_modifier);
                        break;
                }
                
            }
        }
    }

    public override void Dispel(Combatant combatant)
    {
        base.Dispel(combatant);
        
        foreach (var combatMovementInstance in combatant.Hand)
        {
            if (combatMovementInstance is not null)
            {
                switch (_stats)
                {
                    case CombatantMoveStats.Cost:
                        combatMovementInstance.Cost.Amount.RemoveModifier(_modifier);
                        break;
                }
                
            }
        }
    }
}