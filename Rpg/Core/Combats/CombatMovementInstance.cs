namespace Core.Combats;

public sealed class CombatMovementInstance
{
    public CombatMovementInstance(CombatMovement sourceMovement)
    {
        SourceMovement = sourceMovement;
        Effects = sourceMovement.Effects.Select(x => x.CreateInstance()).ToArray();
        AutoDefenseEffects = sourceMovement.AutoDefenseEffects.Select(x => x.CreateInstance()).ToArray();
    }

    public IReadOnlyCollection<IEffectInstance> AutoDefenseEffects { get; }

    public IReadOnlyCollection<IEffectInstance> Effects { get; }

    public CombatMovement SourceMovement { get; }
}