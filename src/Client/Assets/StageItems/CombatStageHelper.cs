using System.Linq;

using Client.Core;

namespace Client.Assets.StageItems;

internal static class CombatStageHelper
{
    public static CombatMetadata CreateMetadata(CombatSource combat)
    {
        var sumPts = combat.Monsters.Sum(x => x.Perks.Count + 1);
        var estimateDifficulty = CalcDifficulty(sumPts);

        return CreateMetadata(combat, estimateDifficulty);
    }

    public static CombatMetadata CreateMetadata(CombatSource combat, CombatEstimateDifficulty combatEstimateDifficulty)
    {
        var leaderPrefab = combat.Monsters.OrderByDescending(x => x.Perks.Count).First();

        var metadata = new CombatMetadata(leaderPrefab.TemplatePrefab, combatEstimateDifficulty);
        return metadata;
    }

    private static CombatEstimateDifficulty CalcDifficulty(int sumPts)
    {
        return sumPts switch
        {
            > 4 => CombatEstimateDifficulty.Hard,
            _ => CombatEstimateDifficulty.Easy,
        };
    }
}