namespace Core.Combats;

public sealed record CombatMovementCost(int Value)
{
    public bool HasCost => Value > 0;
}