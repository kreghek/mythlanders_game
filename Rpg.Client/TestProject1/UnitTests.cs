using System;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace TestProject1
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void TakeDamage_UnitHasTransformation_EventFired()
        {
            // ARRANGE

            var nextScheme = new UnitScheme();

            var sourceScheme = new UnitScheme
            {
                SchemeAutoTransition = new UnitSchemeAutoTransition
                {
                    HpShare = 0.5f,
                    NextScheme = nextScheme
                }
            };

            var unit = new Unit(sourceScheme, 0);

            var halfOfHitPoints = Math.Round(unit.MaxHitPoints * 0.5f, MidpointRounding.AwayFromZero);
            var damage = (int)halfOfHitPoints;

            var damageDealer = Mock.Of<ICombatUnit>();

            // ACT

            using var monitor = unit.Monitor();
            unit.TakeDamage(damageDealer, damage);

            // ASSERT

            monitor.Should().Raise(nameof(unit.SchemeAutoTransition));
        }

        [Test]
        public void TakeDamage_UnitHasTransformation_SchemeChanged()
        {
            // ARRANGE

            var nextScheme = new UnitScheme();

            var sourceScheme = new UnitScheme
            {
                SchemeAutoTransition = new UnitSchemeAutoTransition
                {
                    HpShare = 0.5f,
                    NextScheme = nextScheme
                }
            };

            var unit = new Unit(sourceScheme, 0);

            var halfOfHitPoints = Math.Round(unit.MaxHitPoints * 0.5f, MidpointRounding.AwayFromZero);
            var damage = (int)halfOfHitPoints;

            var damageDealer = Mock.Of<ICombatUnit>();

            // ACT

            unit.TakeDamage(damageDealer, damage);

            // ASSERT

            unit.UnitScheme.Should().BeSameAs(nextScheme);
        }

        [Test]
        public void Constructor_SkillOn1lvl_SkillInTheList()
        {
            // ARRANGE

            var skill = Mock.Of<ISkill>();
            
            var scheme = new UnitScheme
            {
                Levels = new []
                {
                    new AddSkillUnitLevel(1, skill)
                }
            };
            
            // ACT
            
            var unit = new Unit(scheme, 1);
            
            // ASSERT

            unit.Skills.Should().ContainSingle(x=>x == skill);
        }
        
        [Test]
        public void Constructor_SkillOn1lvlAndUnitHas2lvl_SkillInTheList()
        {
            // ARRANGE

            var skill = Mock.Of<ISkill>();
            
            var scheme = new UnitScheme
            {
                Levels = new []
                {
                    new AddSkillUnitLevel(1, skill)
                }
            };
            
            // ACT
            
            var unit = new Unit(scheme, 2);
            
            // ASSERT

            unit.Skills.Should().ContainSingle(x=>x == skill);
        }
        
        [Test]
        public void Constructor_SkillAndPerkOn1lvl_SkillAndPerkInTheLists()
        {
            // ARRANGE

            var skill = Mock.Of<ISkill>();
            var perk = Mock.Of<IPerk>();
            
            var scheme = new UnitScheme
            {
                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel(1, skill),
                    new AddPerkUnitLevel(1, perk)
                }
            };
            
            // ACT
            
            var unit = new Unit(scheme, 1);
            
            // ASSERT

            unit.Skills.Should().ContainSingle(x=>x == skill);
            unit.Perks.Should().ContainSingle(x=>x == perk);
            
        }
    }
}