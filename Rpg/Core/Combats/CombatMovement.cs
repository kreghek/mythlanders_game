namespace Core.Combats;

public sealed class CombatMovement
{
    public CombatMovement(string sid, CombatMovementEffectConfig effectConfig)
    {
        Sid = sid;
        Effects = effectConfig.Effects;
        AutoDefenseEffects = effectConfig.AutoDefenseEffects;
    }

    public string Sid { get; }
    public IReadOnlyCollection<IEffect> Effects { get; }
    public IReadOnlyCollection<IEffect> AutoDefenseEffects { get; }

    public CombatMovementTags Tags { get; set; }
}

public sealed record CombatMovementEffectConfig(IReadOnlyCollection<IEffect> Effects,
    IReadOnlyCollection<IEffect> AutoDefenseEffects)
{
    public static CombatMovementEffectConfig Create(IReadOnlyCollection<IEffect> effects)
    {
        return new CombatMovementEffectConfig(effects, ArraySegment<IEffect>.Empty);
    }
};

[Flags]
public enum CombatMovementTags
{
    None = 0,
    Attack = 1 << 0,
    AutoDefense = 1 << 1
}