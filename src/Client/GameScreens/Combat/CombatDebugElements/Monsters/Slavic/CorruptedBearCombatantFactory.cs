using Client.Assets.CombatMovements.Monster.Slavic.CorruptedBear;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

public class CorruptedBearCombatantFactory : MonsterCombatantFactoryBase
{
    protected override string ClassSid => "corruptedbear";

    protected override CombatantStatsConfig CombatantStatsConfig()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.Resolve, 5);

        return stats;
    }

    protected override CombatMovementSequence CombatMovementSequence(int variationIndex)
    {
        var monsterSequence = CreateCombatMoveVariation(variationIndex);

        return monsterSequence;
    }

    private static CombatMovementSequence CreateCombatMoveVariation(int variationIndex)
    {
        var moveTemplate = new[,]
        {
            { new DevastatingPawFactory().CreateMovement() },
            { new CoverOfDewFactory().CreateMovement() },
            { new HumanSlayerFactory().CreateMovement() },
            { new LordOfNonHumanRealmFactory().CreateMovement() },
            { new RevengeOfWildEyeFactory().CreateMovement() }
        };

        var monsterSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < moveTemplate.GetLength(0); j++)
            {
                var combatMovement = moveTemplate[j, variationIndex];
                monsterSequence.Items.Add(combatMovement);
            }
        }

        return monsterSequence;
    }
}