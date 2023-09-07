namespace Client.Core;

internal static class MonsterCombatantTemplateLevels
{
    public static MonsterCombatantTempateLevel Easy => new(0);
    public static MonsterCombatantTempateLevel Hard => new(2);
    public static MonsterCombatantTempateLevel Medium => new(1);
}