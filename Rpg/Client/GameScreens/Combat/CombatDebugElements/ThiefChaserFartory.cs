using System.Collections.Generic;

using Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class ThiefChaserFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        var list = new List<CombatMovement>
        {
            new ChainHitFactory().CreateMovement(),

            new DoubleKapeshFactory().CreateMovement(),

            new ChasingFactory().CreateMovement(),

            new GuardianPromiseFactory().CreateMovement(),

            new AfterlifeWhirlwindFactory().CreateMovement()
        };

        var monsterSequence = new CombatMovementSequence();
        for (var i = 0; i < 2; i++)
        {
            foreach (var movement in list)
            {
                monsterSequence.Items.Add(movement);
            }
        }

        var stats = new CombatantStatsConfig();
        stats.SetValue(UnitStatType.HitPoints, 5);
        stats.SetValue(UnitStatType.ShieldPoints, 5);
        stats.SetValue(UnitStatType.Resolve, 7);

        var monster = new Combatant("chaser", monsterSequence, stats, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}