namespace Core.Combats;

public class CombatField
{
    public CombatFieldSide HeroSide { get; }
    public CombatFieldSide MonsterSide { get; }

    public CombatField()
    {
        HeroSide = new CombatFieldSide();
        MonsterSide = new CombatFieldSide();
    }
}