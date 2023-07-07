using System;

using Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

using Core.Combats;
using Core.Combats.CombatantStatus;

namespace Client.GameScreens.Combat.CombatDebugElements;

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

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var monsterSequence = CreateCombatMoveVariation();

        var stats = new CombatantStatsConfig();
        stats.SetValue(ICombatantStatType.HitPoints, 6);
        stats.SetValue(ICombatantStatType.ShieldPoints, 3);
        stats.SetValue(ICombatantStatType.Resolve, 4);

        var monster = new Combatant("digitalwolf", monsterSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}