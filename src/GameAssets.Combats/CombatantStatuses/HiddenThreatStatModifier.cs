using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatuses;

internal sealed class HiddenThreatStatModifier: IStatModifier
{
    public IStatModifierSource Source { get; }
    public int Value { get; private set; }

    public HiddenThreatStatModifier(IStatModifierSource source)
    {
        Source = source;
    }

    public void IncrementValue()
    {
        Value++;
    }

    public void ClearValue()
    {
        Value = 0;
    }
}