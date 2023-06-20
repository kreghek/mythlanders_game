using Client.Assets.CombatMovements.Monster.Greek.Automaur;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class AutomataurCombatantFactory : IMonsterCombatantFactory
{
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

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex)
    {
        var monsterSequence = CreateCombatMoveVariation(variationIndex);

        var stats = new CombatantStatsConfig();
        stats.SetValue(UnitStatType.HitPoints, 6);
        stats.SetValue(UnitStatType.ShieldPoints, 4);
        stats.SetValue(UnitStatType.Resolve, 5);

        var monster = new Combatant("automataur", monsterSequence, stats, combatActorBehaviour)
        {
            DebugSid = sid,
            IsPlayerControlled = false
        };

        return monster;
    }
}