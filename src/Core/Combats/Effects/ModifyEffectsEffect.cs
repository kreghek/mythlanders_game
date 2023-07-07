namespace Core.Combats.Effects;

public sealed class ModifyEffectsEffect : IEffect
{
    private readonly ICombatantStatusSid _effectSid;

    public ModifyEffectsEffect(ICombatantStatusSid effectSid, ITargetSelector selector, int value)
    {
        _effectSid = effectSid;
        Selector = selector;
        Value = value;
    }

    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new ModifyEffectsEffectInstance(_effectSid, this);
    }
}