namespace Core.Combats.CombatantEffects;

public sealed class ModifyCombatantMoveStatsCombatantEffect : CombatantEffectBase
{
    private readonly StatModifier _modifier;
    private readonly CombatantMoveStats _stats;

    public ModifyCombatantMoveStatsCombatantEffect(ICombatantEffectSid sid,
        ICombatantEffectLifetime lifetime,
        CombatantMoveStats stats,
        int value) : base(sid, lifetime)
    {
        _stats = stats;

        _modifier = new StatModifier(value);
    }

    public override void Dispel(Combatant combatant)
    {
        base.Dispel(combatant);

        var allCombatMoves = GetAllCombatMoves(combatant);

        foreach (var combatMovementInstance in allCombatMoves)
        {
            switch (_stats)
            {
                case CombatantMoveStats.Cost:
                    combatMovementInstance.Cost.Amount.RemoveModifier(_modifier);
                    break;
            }
        }
    }

    public override void Impose(Combatant combatant, ICombatantEffectImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        var allCombatMoves = GetAllCombatMoves(combatant);

        foreach (var combatMovementInstance in allCombatMoves)
        {
            switch (_stats)
            {
                case CombatantMoveStats.Cost:
                    combatMovementInstance.Cost.Amount.AddModifier(_modifier);
                    break;
            }
        }
    }

    private static IEnumerable<CombatMovementInstance> GetAllCombatMoves(Combatant combatant)
    {
        return combatant.Hand.Where(x => x is not null).Select(x => x!).Concat(combatant.Pool);
    }
}