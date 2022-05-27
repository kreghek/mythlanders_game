using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Assets.Perks;
using Rpg.Client.Core;

namespace Rpg.Client.Tests.Core
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

            var bossUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                MinRequiredBiomeLevel = 1,
                LocationSids = new[] { GlobeNodeSid.Castle },
                Levels = new[]
                {
                    new AddPerkUnitLevel(1, new BossMonster(1))
                }
            };

            var regularUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                LocationSids = new[] { GlobeNodeSid.Castle }
            };

            var predefinedMonsters = new[]
            {
                regularUnitScheme,
                bossUnitScheme
            };
            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == predefinedMonsters);

            var globeContext = Mock.Of<IMonsterGenerationGlobeContext>(x => x.GlobeProgressLevel == 0 &&
                                                                            x.BiomesWithBosses == new[]
                                                                            {
                                                                                BiomeType.Undefined
                                                                            });

            // ACT

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, dice, 8, unitCatalog, globeContext);

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

            var bossUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                MinRequiredBiomeLevel = 0,
                LocationSids = new[] { GlobeNodeSid.Castle },
                Levels = new[]
                {
                    new AddPerkUnitLevel(1, new BossMonster(1))
                }
            };

            var regularUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                LocationSids = new[] { GlobeNodeSid.Castle }
            };

            var predefinedMonsters = new[]
            {
                regularUnitScheme,
                bossUnitScheme
            };
            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == predefinedMonsters);

            var globeContext = Mock.Of<IMonsterGenerationGlobeContext>(x => x.GlobeProgressLevel == 0 &&
                                                                            x.BiomesWithBosses == new[]
                                                                            {
                                                                                BiomeType.Undefined
                                                                            });

            // ACT

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, dice, 8, unitCatalog, globeContext);

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

            var bigUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                LocationSids = new[] { GlobeNodeSid.Battleground },
                Levels = new[]
                {
                    new AddPerkUnitLevel(1, new BigMonster())
                }
            };

            var regularUnitScheme = new UnitScheme(new CommonUnitBasics())
            {
                LocationSids = new[] { GlobeNodeSid.Battleground }
            };

            var predefinedMonsters = new[]
            {
                regularUnitScheme,
                bigUnitScheme
            };
            var unitCatalog = Mock.Of<IUnitSchemeCatalog>(x => x.AllMonsters == predefinedMonsters);

            var globeContext = Mock.Of<IMonsterGenerationGlobeContext>(x => x.GlobeProgressLevel == 0 &&
                                                                            x.BiomesWithBosses == new[]
                                                                            {
                                                                                BiomeType.Undefined
                                                                            });

            // ACT

            var factMonsters =
                MonsterGeneratorHelper.CreateMonsters(node, dice, monsterLevel: default, unitCatalog,
                    globeContext);

            // ASSERT

            factMonsters[0].UnitScheme.Should().Be(bigUnitScheme);
        }
    }
}