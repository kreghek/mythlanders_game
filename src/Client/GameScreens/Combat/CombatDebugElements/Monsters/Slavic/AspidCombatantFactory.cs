using Client.Assets.CombatMovements.Monster.Slavic.Aspid;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

public class AspidCombatantFactory : MonsterCombatantFactoryBase
{
    private static CombatMovementSequence CreateCombatMoveVariation(int variationIndex)
    {
        var moveTemplate = new[,]
        {
            { new SerpentTrapFactory().CreateMovement(), new SpiralInversionFactory().CreateMovement() },

            { new DarkRaidsFactory().CreateMovement(), new EyesOfChaosFactory().CreateMovement() },

            { new SpiralInversionFactory().CreateMovement(), new EbonySkinFactory().CreateMovement() },

            { new EbonySkinFactory().CreateMovement(), new DarkRaidsFactory().CreateMovement() },

            { new EyesOfChaosFactory().CreateMovement(), new SerpentTrapFactory().CreateMovement() }
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

    protected override string ClassSid => "aspid";
}