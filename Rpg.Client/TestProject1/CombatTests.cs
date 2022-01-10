using System.Linq;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace TestProject1
{
    [TestFixture]
    public class CombatTests
    {
        [Test]
        public void UseSkill_PlayerAttacks_MonsterHitPointsWasReduced()
        {
            // ARRANGE

            var playerGroup = new Group();
            var unitScheme = new UnitScheme
            {
                DamageDealerRank = 1,
                Levels = new []
                {
                    new AddSkillUnitLevel(1, new MonsterAttackSkill())
                }
            };

            playerGroup.Slots[0].Unit = new Unit(unitScheme, 1) { IsPlayerControlled = true };

            var globeNode = new GlobeNode();

            var combatSource = new CombatSource
            {
                EnemyGroup = new Group()
            };
            combatSource.EnemyGroup.Slots[0].Unit = new Unit(unitScheme, 1) { IsPlayerControlled = false };

            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

            var combat = new Combat(playerGroup, globeNode, combatSource, new Biome(0, BiomeType.Slavic), dice,
                isAutoplay: false);

            combat.Initialize();
            combat.Update();
            combat.ActionGenerated += (_, args) =>
            {
                args.Action();
            };

            // ACT
            var attacker = combat.CurrentUnit.Unit;
            var skill = attacker.Skills[0];
            var target = combat.AliveUnits.Single(x => x.Unit != attacker);
            var targetSourceHitPoints = target.Unit.HitPoints;

            combat.UseSkill(skill, target);

            // ASSERT
            var targetCurrentHitPoints = target.Unit.HitPoints;
            targetCurrentHitPoints.Should().BeLessThan(targetSourceHitPoints);
        }

        [Test]
        public void UseSkill_PlayerAttacksAndMonsterInDefense_MonsterDefenseReducesDamage()
        {
            // ARRANGE

            var playerGroup = new Group();
            var unitScheme = new UnitScheme
            {
                DamageDealerRank = 1,
                Levels = new []
                {
                    new AddSkillUnitLevel(1, new MonsterAttackSkill())
                }
            };

            var monsterUnitScheme = new UnitScheme
            {
                Levels = new []
                {
                    new AddSkillUnitLevel(1, new DefenseStanceSkill())
                }
            };

            playerGroup.Slots[0].Unit = new Unit(unitScheme, 1) { IsPlayerControlled = true };

            var globeNode = new GlobeNode();

            var combatSource = new CombatSource
            {
                EnemyGroup = new Group()
            };
            combatSource.EnemyGroup.Slots[0].Unit = new Unit(monsterUnitScheme, 1) { IsPlayerControlled = false };

            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

            var combat = new Combat(playerGroup, globeNode, combatSource, new Biome(0, BiomeType.Slavic), dice,
                isAutoplay: false);

            combat.Initialize();
            combat.Update();
            combat.ActionGenerated += (_, args) =>
            {
                args.Action();
            };

            // ACT 1
            var attacker = combat.CurrentUnit.Unit;
            var skill = attacker.Skills.First();
            var target = combat.AliveUnits.Single(x => x.Unit != attacker);
            var targetSourceHitPoints = target.Unit.HitPoints;

            combat.UseSkill(skill, target);

            var targetCurrentHitPoints = target.Unit.HitPoints;
            combat.Update();

            // // ACT 2
            // var attacker2 = combat.CurrentUnit;
            // var skill2 = attacker.Skills.First();
            // combat.UseSkill(skill2, attacker2);
            combat.Update();

            // ACT 3
            var attacker3 = combat.CurrentUnit.Unit;
            var skill3 = attacker3.Skills.First();
            var target3 = combat.AliveUnits.Single(x => x.Unit != attacker);
            var targetSourceHitPoints3 = target3.Unit.HitPoints;

            combat.UseSkill(skill3, target3);
            combat.Update();

            // ASSERT
            var targetCurrentHitPoints3 = target.Unit.HitPoints;
            var targetHitPointsDiff = targetSourceHitPoints - targetCurrentHitPoints;
            var targetHitPointsDiff3 = targetSourceHitPoints3 - targetCurrentHitPoints3;
            targetHitPointsDiff3.Should().NotBe(0).And.BeLessThan(targetHitPointsDiff);
        }
    }
}