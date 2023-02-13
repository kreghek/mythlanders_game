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

    public IReadOnlyCollection<IEffect> AutoDefenseEffects { get; }
    public CombatMovementCost Cost { get; }
    public IReadOnlyCollection<IEffect> Effects { get; }

    public string Sid { get; }

    public CombatMovementTags Tags { get; set; }
}