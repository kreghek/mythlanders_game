﻿using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Rpg.Client.Core.Tests
{
    [TestFixture]
    public class MonsterGeneratorHelperTests
    {
        [Test]
        public void CreateMonsters_LastNodeInCompleteBiome_ReturnsRegularMonster()
        {
            // ARRANGE

            var node = new GlobeNode
            {
                IsLast = true,
                Sid = GlobeNodeSid.Castle
            };

            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

            var biome = new Biome(default, default)
            {
                Level = 0,
                IsComplete = true
            };

            var bossUnitScheme = new UnitScheme
            {
                BossLevel = 1,
                MinRequiredBiomeLevel = 0,
                LocationSids = new[] { GlobeNodeSid.Castle }
            };

            var regularUnitScheme = new UnitScheme
            {
                BossLevel = null,
                LocationSids = new[] { GlobeNodeSid.Castle }
            };

            var predefinedMonsters = new[]
            {
                regularUnitScheme,
                bossUnitScheme
            };
            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == predefinedMonsters);

            // ACT

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, dice, biome, 8, unitCatalog);

            // ASSERT

            factMonsters[0].UnitScheme.Should().Be(regularUnitScheme);
        }

        [Test]
        public void CreateMonsters_LastNodeInIncompleteBiome_ReturnsBoss()
        {
            // ARRANGE

            var node = new GlobeNode
            {
                IsLast = true,
                Sid = GlobeNodeSid.Castle
            };

            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

            var biome = new Biome(default, default)
            {
                Level = 0,
                IsComplete = false
            };

            var bossUnitScheme = new UnitScheme
            {
                BossLevel = 1,
                MinRequiredBiomeLevel = 0,
                LocationSids = new[] { GlobeNodeSid.Castle }
            };

            var regularUnitScheme = new UnitScheme
            {
                BossLevel = null,
                LocationSids = new[] { GlobeNodeSid.Castle }
            };

            var predefinedMonsters = new[]
            {
                regularUnitScheme,
                bossUnitScheme
            };
            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == predefinedMonsters);

            // ACT

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, dice, biome, 8, unitCatalog);

            // ASSERT

            factMonsters[0].UnitScheme.Should().Be(bossUnitScheme);
        }

        [Test]
        public void CreateMonsters_RollBigAndRegularMonsters_ReturnsOnlyOneBigMonster()
        {
            // ARRANGE

            var node = new GlobeNode
            {
                Sid = GlobeNodeSid.Battleground
            };

            // Roll 2 to get non-single monster count.
            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 2);

            var biome = new Biome(minLevel: default, type: default);

            var bigUnitScheme = new UnitScheme
            {
                LocationSids = new[] { GlobeNodeSid.Battleground },
                IsBig = true
            };

            var regularUnitScheme = new UnitScheme
            {
                LocationSids = new[] { GlobeNodeSid.Battleground }
            };

            var predefinedMonsters = new[]
            {
                regularUnitScheme,
                bigUnitScheme
            };
            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == predefinedMonsters);

            // ACT

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, dice, biome, combatLevel: default, unitCatalog);

            // ASSERT

            factMonsters[0].UnitScheme.Should().Be(bigUnitScheme);
        }
    }
}