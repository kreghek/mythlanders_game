namespace Core.Combats;

public sealed class CombatMovement
{
    public CombatMovement(string sid, CombatMovementCost cost, CombatMovementEffectConfig effectConfig)
    {
        Sid = sid;
        Cost = cost;
        Effects = effectConfig.Effects;
        AutoDefenseEffects = effectConfig.AutoDefenseEffects;
    }

    public string Sid { get; }
    public CombatMovementCost Cost { get; }
    public IReadOnlyCollection<IEffect> Effects { get; }
    public IReadOnlyCollection<IEffect> AutoDefenseEffects { get; }

    public CombatMovementTags Tags { get; set; }
}

public sealed record CombatMovementCost(int Value);