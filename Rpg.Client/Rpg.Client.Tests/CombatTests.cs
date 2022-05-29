using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Tests
{
    [TestFixture]
    public class CombatTests
    {
        [Test]
        public void DemoBalance_VolkolakDealsNonZeroDamage()
        {
            // ARRANGE

            var demoUnitCatalog = new DemoUnitSchemeCatalog();

            var playerGroup = new Group();
            var unitScheme = GetVolkolakBeastFormScheme(demoUnitCatalog);

            var monsterUnitScheme = new UnitScheme(new CommonUnitBasics());

            playerGroup.Slots[0].Unit = new Unit(unitScheme, 5) { IsPlayerControlled = true };

            var globeNode = new GlobeNode();

            var combatSource = new CombatSource
            {
                EnemyGroup = new Group()
            };
            combatSource.EnemyGroup.Slots[0].Unit = new Unit(monsterUnitScheme, 1) { IsPlayerControlled = false };

            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

            var combat = new Combat(playerGroup, globeNode, combatSource, dice, isAutoplay: false);
            using var monitor = combat.Monitor();

            combat.Initialize();

            combat.Update();
            combat.ActionGenerated += (_, args) =>
            {
                var skillExecution = args.Action;
                HandleInteraction(skillExecution);
            };

            // ACT
            var attacker = combat.CurrentUnit;
            var skill = attacker.CombatCards.First();
            var target = combat.AliveUnits.Single(x => x != attacker);
            var takenDamageMount = 0;
            target.HasTakenHitPointsDamage += (_, e) =>
            {
                takenDamageMount = e.Amount;
            };

            combat.UseSkill(skill, target);

            // ASSERT
            takenDamageMount.Should().BeGreaterThan(2);
        }

        [Test]
        public void UseSkill_PeriodicDamageDefeatsMonster_NotThrowException()
        {
            // ARRANGE

            var hugePeriodicDamageRule = new List<EffectRule>
            {
                new EffectRule
                {
                    Direction = SkillDirection.Target,
                    EffectCreator = new EffectCreator(unit =>
                    {
                        return new PeriodicDamageEffect(unit, new DurationEffectLifetime(new EffectDuration(1)))
                        {
                            PowerMultiplier = 10000,
                            SourceDamage = 1
                        };
                    })
                }
            };

            var playerGroup = new Group();
            var unitScheme = new UnitScheme(new CommonUnitBasics())
            {
                DamageDealerRank = 1,
                Levels = new[]
                {
                    new AddSkillUnitLevel(1,
                        Mock.Of<ISkill>(skill =>
                            // ReSharper disable once PossibleUnintendedReferenceComparison
                            // Justification: Used to mock creating.
                            skill.Rules == hugePeriodicDamageRule && skill.TargetType == SkillTargetType.Enemy)
                    )
                }
            };

            var monsterUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                Levels = new[]
                {
                    new AddSkillUnitLevel(1,
                        Mock.Of<ISkill>(skill =>
                            // ReSharper disable once PossibleUnintendedReferenceComparison
                            // Justification: Used to mock creating.
                            skill.Rules == hugePeriodicDamageRule && skill.TargetType == SkillTargetType.Enemy)
                    )
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

            var combat = new Combat(playerGroup, globeNode, combatSource, dice, isAutoplay: false);
            using var monitor = combat.Monitor();

            var finishEventWasRaised = false;
            combat.Finish += (_, _) =>
            {
                finishEventWasRaised = true;
            };

            combat.Initialize();
            combat.Update();
            combat.ActionGenerated += DefaultActionGeneratedEventHandler;

            // ACT
            var attacker = combat.CurrentUnit;
            var skill = attacker.CombatCards.First();
            var target = combat.AliveUnits.Single(x => x != attacker);

            combat.UseSkill(skill, target);

            combat.Update();

            combat.Update();

            // ASSERT
            target.IsDead.Should().BeTrue();
            finishEventWasRaised.Should().BeTrue();
        }

        [Test]
        public void UseSkill_PlayerAttacks_MonsterHitPointsWasReduced()
        {
            // ARRANGE

            var playerGroup = new Group();

            var damageRule = new List<EffectRule>
            {
                new EffectRule
                {
                    Direction = SkillDirection.Target,
                    EffectCreator = new EffectCreator(unit =>
                    {
                        return new DamageEffect
                        {
                            Actor = unit,
                            DamageMultiplier = 1
                        };
                    })
                }
            };

            var unitScheme = new UnitScheme(new CommonUnitBasics())
            {
                DamageDealerRank = 1,
                Levels = new[]
                {
                    new AddSkillUnitLevel(1, Mock.Of<ISkill>(skill =>
                        // ReSharper disable once PossibleUnintendedReferenceComparison
                        // Justification Creating mock using the expression tree.
                        skill.Rules == damageRule && skill.TargetType == SkillTargetType.Enemy))
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

            var combat = new Combat(playerGroup, globeNode, combatSource, dice, isAutoplay: false);

            combat.Initialize();
            combat.Update();
            combat.ActionGenerated += DefaultActionGeneratedEventHandler;

            // ACT
            var attacker = combat.CurrentUnit;
            var skill = attacker.CombatCards.First();
            var target = combat.AliveUnits.Single(x => x != attacker);
            var targetSourceHitPoints = target.Stats.SingleOrDefault(x => x.Type == UnitStatType.HitPoints).Value.Current;

            combat.UseSkill(skill, target);

            // ASSERT
            var targetCurrentHitPoints = target.Stats.SingleOrDefault(x => x.Type == UnitStatType.HitPoints).Value.Current;
            targetCurrentHitPoints.Should().BeLessThan(targetSourceHitPoints);
        }

        [Test]
        public void UseSkill_PlayerAttacksAndMonsterInDefense_MonsterDefenseReducesDamage()
        {
            // ARRANGE

            var playerGroup = new Group();

            var damageRule = new List<EffectRule>
            {
                new EffectRule
                {
                    Direction = SkillDirection.Target,
                    EffectCreator = new EffectCreator(unit =>
                    {
                        return new DamageEffect
                        {
                            Actor = unit,
                            DamageMultiplier = 1
                        };
                    })
                }
            };

            var unitScheme = new UnitScheme(new CommonUnitBasics())
            {
                DamageDealerRank = 1,
                Levels = new[]
                {
                    new AddSkillUnitLevel(1, Mock.Of<ISkill>(skill =>
                        // ReSharper disable once PossibleUnintendedReferenceComparison
                        // Justification Creating mock using the expression tree.
                        skill.Rules == damageRule && skill.TargetType == SkillTargetType.Enemy))
                }
            };

            var decreaseDamageRule = new List<EffectRule>
            {
                new EffectRule
                {
                    Direction = SkillDirection.Target,
                    EffectCreator = new EffectCreator(unit =>
                    {
                        return new ProtectionEffect(unit, new DurationEffectLifetime(new EffectDuration(1)),
                            multiplier: 0f);
                    })
                }
            };

            var monsterUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                Levels = new[]
                {
                    new AddSkillUnitLevel(1, Mock.Of<ISkill>(skill =>
                        // ReSharper disable once PossibleUnintendedReferenceComparison
                        // Justification Creating mock using the expression tree.
                        skill.Rules == decreaseDamageRule && skill.TargetType == SkillTargetType.Enemy))
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

            var combat = new Combat(playerGroup, globeNode, combatSource, dice, isAutoplay: false);

            combat.Initialize();
            combat.Update();
            combat.ActionGenerated += DefaultActionGeneratedEventHandler;

            // ACT 1
            var attacker = combat.CurrentUnit;
            var skill = attacker.CombatCards.First();
            var target = combat.AliveUnits.Single(x => x != attacker);
            var targetSourceHitPoints = target.Stats.SingleOrDefault(x => x.Type == UnitStatType.HitPoints).Value.Current;

            combat.UseSkill(skill, target);

            var targetCurrentHitPoints = target.Stats.SingleOrDefault(x => x.Type == UnitStatType.HitPoints).Value.Current;
            // Update combat will move turn to next unit in the queue.
            // It will invoke Ai-turn.
            // Ai will cast defence on yourself. 
            combat.Update();

            // ACT 2
            //combat.Update();

            // ACT 3
            var attacker3 = combat.CurrentUnit;
            var skill3 = attacker3.CombatCards.First();
            var target3 = combat.AliveUnits.Single(x => x != attacker);
            var targetSourceHitPoints3 = target3.Stats.SingleOrDefault(x => x.Type == UnitStatType.HitPoints).Value.Current;

            combat.UseSkill(skill3, target3);
            combat.Update();

            // ASSERT
            var targetCurrentHitPoints3 = target.Stats.SingleOrDefault(x => x.Type == UnitStatType.HitPoints).Value.Current;
            var targetHitPointsDiff = targetSourceHitPoints - targetCurrentHitPoints;
            var targetHitPointsDiff3 = targetSourceHitPoints3 - targetCurrentHitPoints3;
            targetHitPointsDiff3.Should().BeLessThan(targetHitPointsDiff);
        }

        private static void DefaultActionGeneratedEventHandler(object _, ActionEventArgs args)
        {
            var skillExecution = args.Action;
            HandleInteraction(skillExecution);
        }

        private static UnitScheme GetVolkolakBeastFormScheme(IUnitSchemeCatalog demoUnitCatalog)
        {
            var volkolakTransition = demoUnitCatalog.AllMonsters.Single(x => x.Name == UnitName.VolkolakWarrior)
                .SchemeAutoTransition;

            if (volkolakTransition is null)
            {
                throw new InvalidOperationException();
            }

            return volkolakTransition.NextScheme;
        }

        private static void HandleInteraction(SkillExecution skillExecution)
        {
            var actionSkillRuleInteractions = skillExecution.SkillRuleInteractions;

            foreach (var skillRuleInteraction in actionSkillRuleInteractions)
            {
                foreach (var target in skillRuleInteraction.Targets)
                {
                    skillRuleInteraction.Action(target);
                }
            }

            skillExecution.SkillComplete();
        }
    }
}