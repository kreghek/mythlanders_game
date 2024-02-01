using Client.Assets.CombatMovements.Monster.Greek.Automaur;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Greek;

public class PaintedSkinCombatantFactory : MonsterCombatantFactoryBase
{
    protected override string ClassSid => "paintedskin";

    protected override CombatantStatsConfig CombatantStatsConfig()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 6);
        stats.SetValue(CombatantStatTypes.Resolve, 7);

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
            { new WheelOfDeathFactory().CreateMovement() },

            { new RelentlessRacerFactory().CreateMovement() },

            { new HuntingForLivesFactory().CreateMovement() },

            { new OctaneBloodFactory().CreateMovement() },

            { new SpeedFrenzyFactory().CreateMovement() }
        };

        var monsterSequence = new CombatMovementSequence();

        for (var i = 0; i < 1; i++)
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