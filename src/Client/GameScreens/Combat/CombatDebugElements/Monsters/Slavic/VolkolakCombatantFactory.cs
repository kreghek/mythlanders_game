using Client.Assets.CombatMovements.Monster.Egyptian.Chaser;
using Client.Assets.CombatMovements.Monster.Slavic.Chaser;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

public class VolkolakCombatantFactory : MonsterCombatantFactoryBase
{
    protected override string ClassSid => "volkolakwarrior";

    protected override CombatantStatsConfig CombatantStatsConfig()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 6);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 4);
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
            { new ChainHitFactory().CreateMovement(), new ChasingFactory().CreateMovement() },

            { new DoubleKapeshFactory().CreateMovement(), new AfterlifeWhirlwindFactory().CreateMovement() },

            { new ChasingFactory().CreateMovement(), new GuardianPromiseFactory().CreateMovement() },

            { new GuardianPromiseFactory().CreateMovement(), new DoubleKapeshFactory().CreateMovement() },

            { new AfterlifeWhirlwindFactory().CreateMovement(), new ChainHitFactory().CreateMovement() }
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