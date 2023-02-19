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

public sealed class CombatMovementInstance
{
    public CombatMovementInstance(CombatMovement sourceMovement)
    {
        SourceMovement = sourceMovement;
        Effects = sourceMovement.Effects.Select(x => x.CreateInstance()).ToArray();
        AutoDefenseEffects = sourceMovement.AutoDefenseEffects.Select(x => x.CreateInstance()).ToArray();
    }

    public CombatMovement SourceMovement { get; }
    
    public IReadOnlyCollection<IEffectInstance> Effects { get; }
    public IReadOnlyCollection<IEffectInstance> AutoDefenseEffects { get; }
}