using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements.Monster.Slavic.CorruptedBear;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

public class CorruptedBearCombatantFactory : IMonsterCombatantFactory
{
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

    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex,
        IReadOnlyCollection<ICombatantStatusFactory> combatantStatusFactories)
    {
        var monsterSequence = CreateCombatMoveVariation(variationIndex);

        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.Resolve, 5);

        var monster = new TestamentCombatant("corruptedbear", monsterSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid,
            IsPlayerControlled = false
        };

        return monster;
    }
}