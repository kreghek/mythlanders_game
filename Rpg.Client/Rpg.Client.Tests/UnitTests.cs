using System.Linq;

using Client.Core;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Tests
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void Constructor_LevelUpEquipmentAffectedToHp_HpRaised()
        {
            // ARRANGE

            var scheme = new UnitScheme(new CommonUnitBasics())
            {
                Equipments = new[]
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    // This is for reflection to make mock.
                    Mock.Of<IEquipmentScheme>(x => x.GetStatModifiers(It.IsAny<int>()) ==
                                                   new (UnitStatType, IUnitStatModifier)[]
                                                   {
                                                       new(UnitStatType.HitPoints, new StatModifier(1))
                                                   })
                }
            };

            // ACT

            var unit = new Unit(scheme, 1);

            // ASSERT
            var expectedValue = (int)(scheme.HitPointsBase * 2f);
            unit.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.ActualMax.Should().Be(expectedValue);
        }

        [Test]
        public void Constructor_ReplaceSkillOn5lvl_SkillReplacedByImrovedVersion()
        {
            // ARRANGE

            var skill = Mock.Of<ISkill>(x => x.Sid == SkillSid.DieBySword);
            var improvedSkill = Mock.Of<ISkill>();

            var scheme = new UnitScheme(new CommonUnitBasics())
            {
                Levels = new IUnitLevelScheme[]
                {
                    new AddPredefinedSkillUnitLevel(1, skill),
                    new ReplaceSkillUnitLevel(5, SkillSid.DieBySword, improvedSkill)
                }
            };

            // ACT

            var unit = new Unit(scheme, 5);

            // ASSERT

            unit.Skills.Should().ContainSingle(x => x == improvedSkill);
        }

        [Test]
        public void Constructor_SkillAndPerkOn1lvl_SkillAndPerkInTheLists()
        {
            // ARRANGE

            var skill = Mock.Of<ISkill>();
            var perk = Mock.Of<IPerk>();

            var scheme = new UnitScheme(new CommonUnitBasics())
            {
                Levels = new IUnitLevelScheme[]
                {
                    new AddPredefinedSkillUnitLevel(1, skill),
                    new AddPredefinedPerkUnitLevel(1, perk)
                }
            };

            // ACT

            var unit = new Unit(scheme, 1);

            // ASSERT

            unit.Skills.Should().ContainSingle(x => x == skill);
            unit.Perks.Should().ContainSingle(x => x == perk);
        }

        [Test]
        public void Constructor_SkillOn1lvl_SkillInTheList()
        {
            // ARRANGE

            var skill = Mock.Of<ISkill>();

            var scheme = new UnitScheme(new CommonUnitBasics())
            {
                Levels = new[]
                {
                    new AddPredefinedSkillUnitLevel(1, skill)
                }
            };

            // ACT

            var unit = new Unit(scheme, 1);

            // ASSERT

            unit.Skills.Should().ContainSingle(x => x == skill);
        }

        [Test]
        public void Constructor_SkillOn1lvlAndUnitHas2lvl_SkillInTheList()
        {
            // ARRANGE

            var skill = Mock.Of<ISkill>();

            var scheme = new UnitScheme(new CommonUnitBasics())
            {
                Levels = new[]
                {
                    new AddPredefinedSkillUnitLevel(1, skill)
                }
            };

            // ACT

            var unit = new Unit(scheme, 2);

            // ASSERT

            unit.Skills.Should().ContainSingle(x => x == skill);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(10, 10)]
        public void ShieldPoints_DifferentLevels_ShieldPointsGrowWithLevel(int level, int expectedMaxSp)
        {
            // ARRANGE
            var basics = new CommonUnitBasics
            {
                ARMOR_BASE = 1,
                POWER_BASE = 0,
                POWER_PER_LEVEL_BASE = 1,
                HERO_POWER_MULTIPLICATOR = 1
            };
            var sourceScheme = new UnitScheme(basics)
            {
                TankRank = 1f
            };

            var unit = new Unit(sourceScheme, level);

            // ACT

            var factSp = unit.Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value;

            // ASSERT

            factSp.ActualMax.Should().Be(expectedMaxSp);
        }

        //[Test]
        //public void TakeDamage_UnitHasTransformation_EventFired()
        //{
        //    // ARRANGE

        //    var nextScheme = new UnitScheme(new CommonUnitBasics());

        //    var sourceScheme = new UnitScheme(new CommonUnitBasics())
        //    {
        //        SchemeAutoTransition = new UnitSchemeAutoTransition
        //        {
        //            HpShare = 0.5f,
        //            NextScheme = nextScheme
        //        }
        //    };

        //    var unit = new Unit(sourceScheme, 0);

        //    var halfOfHitPoints = Math.Round(unit.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.ActualMax * 0.5f, MidpointRounding.AwayFromZero);
        //    var damage = (int)halfOfHitPoints;

        //    var damageDealer = Mock.Of<ICombatUnit>();

        //    // ACT

        //    using var monitor = unit.Monitor();
        //    unit.TakeDamage(damageDealer, damage);

        //    // ASSERT

        //    monitor.Should().Raise(nameof(unit.SchemeAutoTransition));
        //}

        //[Test]
        //public void TakeDamage_UnitHasTransformation_SchemeChanged()
        //{
        //    // ARRANGE

        //    var nextScheme = new UnitScheme(new CommonUnitBasics());

        //    var sourceScheme = new UnitScheme(new CommonUnitBasics())
        //    {
        //        SchemeAutoTransition = new UnitSchemeAutoTransition
        //        {
        //            HpShare = 0.5f,
        //            NextScheme = nextScheme
        //        }
        //    };

        //    var unit = new Unit(sourceScheme, 0);

        //    var halfOfHitPoints = Math.Round(unit.HitPoints.ActualMax * 0.5f, MidpointRounding.AwayFromZero);
        //    var damage = (int)halfOfHitPoints;

        //    var damageDealer = Mock.Of<ICombatUnit>();

        //    // ACT

        //    unit.TakeDamage(damageDealer, damage);

        //    // ASSERT

        //    unit.UnitScheme.Should().BeSameAs(nextScheme);
        //}
    }
}