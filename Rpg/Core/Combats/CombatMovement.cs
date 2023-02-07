namespace Core.Combats;

public sealed class CombatMovement
{
    public CombatMovement(string sid, IReadOnlyCollection<IEffect> effects)
    {
        Sid = sid;
        Effects = effects;
    }

    public string Sid { get; }
    public IReadOnlyCollection<IEffect> Effects { get; }
}