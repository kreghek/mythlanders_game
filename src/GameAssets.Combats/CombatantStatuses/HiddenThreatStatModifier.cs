using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatuses;

internal sealed class HiddenThreatStatModifier : IStatModifier
{
    public HiddenThreatStatModifier(IStatModifierSource source)
    {
        Source = source;
    }

    public void ClearValue()
    {
        Value = 0;
    }

    public void IncrementValue()
    {
        Value++;
    }

    public IStatModifierSource Source { get; }
    public int Value { get; private set; }
}