using Client.Assets.CombatMovements.Monster.Black.AmbushDrone;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Black;

public class AmbushDroneCombatantFactory : MonsterCombatantFactoryBase
{
    protected override CombatantStatsConfig CombatantStatsConfig()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 2);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 2);
        stats.SetValue(CombatantStatTypes.Resolve, 4);

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
            { new IronStreamFactory().CreateMovement(), new OminousThornFactory().CreateMovement() },
            { new OminousThornFactory().CreateMovement(), new IronStreamFactory().CreateMovement() },
            { new IronStreamFactory().CreateMovement(), new OminousThornFactory().CreateMovement() },
            { new OminousThornFactory().CreateMovement(), new IronStreamFactory().CreateMovement() },
            { new IronStreamFactory().CreateMovement(), new OminousThornFactory().CreateMovement() }
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