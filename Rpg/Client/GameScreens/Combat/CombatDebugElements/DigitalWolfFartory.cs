using System.Collections.Generic;

using Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class DigitalWolfFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var monsterSequence = CreateCombatMoveVariation();

        var stats = new CombatantStatsConfig();
        stats.SetValue(UnitStatType.HitPoints, 5);
        stats.SetValue(UnitStatType.ShieldPoints, 3);
        stats.SetValue(UnitStatType.Resolve, 4);

        var monster = new Combatant("digitalwolf", monsterSequence, stats, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = false
        };

        return monster;
    }

    private static CombatMovementSequence CreateCombatMoveVariation()
    {
        var list = new CombatMovement[]
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
}