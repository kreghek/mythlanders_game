using System.Collections.Generic;

using Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class DigitalWolfFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new List<CombatMovement>();

        list.Add(new CyberClawsFactory().CreateMovement());
        list.Add(new VelesProtectionFactory().CreateMovement());
        list.Add(new EnergeticBiteFactory().CreateMovement());
        list.Add(new RegenerativeProtocolFactory().CreateMovement());
        list.Add(new FlockAlphaTacticsFactory().CreateMovement());


        var monsterSequence = new CombatMovementSequence();
        for (var i = 0; i < 3; i++)
        {
            foreach (var combatMovement in list)
            {
                monsterSequence.Items.Add(combatMovement);
            }
        }

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
}