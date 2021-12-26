using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Rpg.Client.Core.Tests
{
    [TestFixture]
    public class MonsterGeneratorHelperTests
    {
        [Test]
        public void CreateMonsters_LastNodeInIncompleteBiome_ReturnsBoss()
        {
            // ARRANGE

            var node = new GlobeNode() { IsLast = true, Sid = GlobeNodeSid.Castle };

            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

            var biome = new Biome(default, default) { Level = 0, IsComplete = false };

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

            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == new[] {
                regularUnitScheme,
                bossUnitScheme
            });

            // ACT

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, dice, biome, 8, unitCatalog);

            // ASSERT

            factMonsters[0].UnitScheme.Should().Be(bossUnitScheme);
        }

        [Test]
        public void CreateMonsters_LastNodeInCompleteBiome_ReturnsRegularMonster()
        {
            // ARRANGE

            var node = new GlobeNode() { IsLast = true, Sid = GlobeNodeSid.Castle };

            var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

            var biome = new Biome(default, default) { Level = 0, IsComplete = true };

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

            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == new[] {
                regularUnitScheme,
                bossUnitScheme
            });

            // ACT

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, dice, biome, 8, unitCatalog);

            // ASSERT

            factMonsters[0].UnitScheme.Should().Be(regularUnitScheme);
        }
    }
}