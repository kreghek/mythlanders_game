﻿using Client.Assets.CombatMovements.Monster.Slavic.Aspid;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class AspidFactory : IMonsterCombatantFactory
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

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex)
    {
        var monsterSequence = CreateCombatMoveVariation(variationIndex);

        var stats = new CombatantStatsConfig();
        stats.SetValue(UnitStatType.HitPoints, 6);
        stats.SetValue(UnitStatType.ShieldPoints, 4);
        stats.SetValue(UnitStatType.Resolve, 5);

        var monster = new Combatant("aspid", monsterSequence, stats, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}