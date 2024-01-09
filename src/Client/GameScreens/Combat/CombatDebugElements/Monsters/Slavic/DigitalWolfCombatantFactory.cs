using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

public class DigitalWolfCombatantFactory : IMonsterCombatantFactory
{
    private static CombatMovementSequence CreateCombatMoveVariation()
    {
        var list = new[]
        {
            new CyberClawsFactory().CreateMovement(),
            new VelesProtectionFactory().CreateMovement(),
            new EnergeticBiteFactory().CreateMovement(),
            new RegenerativeProtocolFactory().CreateMovement(),
            new FlockAlphaTacticsFactory().CreateMovement()
        };

        var monsterSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
        {
            foreach (var combatMovement in list)
            {
                monsterSequence.Items.Add(combatMovement);
            }
        }

        return monsterSequence;
    }

    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex,
        IReadOnlyCollection<ICombatantStatusFactory> combatantStatusFactories)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var monsterSequence = CreateCombatMoveVariation();

        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 3);
        stats.SetValue(CombatantStatTypes.Resolve, 4);

        var monster = new TestamentCombatant("digitalwolf", monsterSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}