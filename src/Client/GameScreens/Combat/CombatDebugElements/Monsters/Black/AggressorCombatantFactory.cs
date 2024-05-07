using Client.Assets.CombatMovements.Monster.Black.Aggressor;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Black;

public class AggressorCombatantFactory : MonsterCombatantFactoryBase
{
    protected override string ClassSid => "aggressor";

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
            { new GrimResolveFactory().CreateMovement(), new FatalBlowFactory().CreateMovement() },

            { new DanceWithThePastFactory().CreateMovement(), new LastChanceToPeaceFactory().CreateMovement() },

            { new FatalBlowFactory().CreateMovement(), new MadnessWithinEyesFactory().CreateMovement() },

            { new MadnessWithinEyesFactory().CreateMovement(), new DanceWithThePastFactory().CreateMovement() },

            { new LastChanceToPeaceFactory().CreateMovement(), new GrimResolveFactory().CreateMovement() }
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