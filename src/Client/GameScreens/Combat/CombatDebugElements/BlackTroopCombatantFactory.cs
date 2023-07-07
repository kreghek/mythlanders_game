using System;

using Client.Assets.CombatMovements.Monster.Black.BlackTroop;

using Core.Combats;
using Core.Combats.CombatantStatus;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class BlackTroopCombatantFactory : IMonsterCombatantFactory
{
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

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex)
    {
        var monsterSequence = CreateCombatMoveVariation(variationIndex);

        var stats = new CombatantStatsConfig();
        stats.SetValue(ICombatantStatType.HitPoints, 6);
        stats.SetValue(ICombatantStatType.ShieldPoints, 4);
        stats.SetValue(ICombatantStatType.Resolve, 5);

        var monster = new Combatant("blacktrooper", monsterSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}