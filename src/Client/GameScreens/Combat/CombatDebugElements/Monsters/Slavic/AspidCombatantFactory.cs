using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements.Monster.Slavic.Aspid;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

public class AspidCombatantFactory : IMonsterCombatantFactory
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

    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex,
        IReadOnlyCollection<ICombatantStatusFactory> combatantStatusFactories)
    {
        var monsterSequence = CreateCombatMoveVariation(variationIndex);

        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.Resolve, 5);

        var monster = new TestamentCombatant("aspid", monsterSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid,
            IsPlayerControlled = false
        };

        return monster;
    }
}