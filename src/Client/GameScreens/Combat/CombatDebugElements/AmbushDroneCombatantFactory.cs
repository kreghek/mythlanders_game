using System;

using Client.Assets.CombatMovements.Monster.Black.Agressor;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class AmbushDroneCombatantFactory : IMonsterCombatantFactory
{
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

    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex)
    {
        var monsterSequence = CreateCombatMoveVariation(variationIndex);

        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 2);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 2);
        stats.SetValue(CombatantStatTypes.Resolve, 4);

        var monster = new TestamentCombatant("ambushdrone", monsterSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}