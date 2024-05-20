using System.Linq;

using Client.Assets.Catalogs;
using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Dices;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Client.Tests.Assets;

public class MonsterPerkManagerTests
{
    [Test]
    public void RollLocationRewardPerks_do_not_roll_perks_unique()
    {
        // ARRANGE

        var uniquePerk = new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "black")
        {
            IsUnique = true
        };

        var regularPerk = new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "all");

        var monsterPerkCatalog = Mock.Of<IMonsterPerkCatalog>(x => x.Perks == new[]
        {
            uniquePerk,
            regularPerk,
            new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "ExtraHitPoints"),
            new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "ExtraShieldPoints")
        });

        var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 1);

        var globeProvider = new GlobeProvider(Mock.Of<ICharacterCatalog>(), Mock.Of<IStoryPointInitializer>(),
            monsterPerkCatalog);

        globeProvider.GenerateNew();
        globeProvider.Globe.Player.AddMonsterPerk(uniquePerk);
        globeProvider.Globe.Features.AddFeature(GameFeatures.RewardMonsterPerksCampaignEffect);

        var sut = new MonsterPerkManager(dice, monsterPerkCatalog,
            globeProvider);

        // ACT

        var perks = sut.RollLocationRewardPerks();

        // ASSERT

        perks.Single().Sid.Should().NotBe(uniquePerk.Sid);
    }

    [Theory]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void RollMonsterPerks_do_not_roll_perks_only_for_black_for_other(int rolledIndex)
    {
        // ARRANGE

        var blackPerk = new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "black")
        {
            Predicates = new[]
            {
                new OnlyForBlackConclaveMonsterPerkPredicate()
            }
        };

        var regularPerk = new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "all");

        var monsterPerkCatalog = Mock.Of<IMonsterPerkCatalog>(x => x.Perks == new[]
        {
            blackPerk,
            regularPerk,
            new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "ExtraHitPoints"),
            new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "ExtraShieldPoints")
        });

        var diceMock = new Mock<IDice>();
        diceMock.SetupSequence(x => x.Roll(It.IsAny<int>()))
            .Returns(2) // to roll only single perk (n-1+min, where min == 0, totally == 1 on d2)
            .Returns(rolledIndex);
        var dice = diceMock.Object;

        var globeProvider = new GlobeProvider(Mock.Of<ICharacterCatalog>(), Mock.Of<IStoryPointInitializer>(),
            monsterPerkCatalog);

        globeProvider.GenerateNew();

        globeProvider.Globe.Player.AddMonsterPerk(blackPerk);
        globeProvider.Globe.Player.AddMonsterPerk(regularPerk);
        globeProvider.Globe.Features.AddFeature(GameFeatures.UseMonsterPerks);

        var monster = new MonsterCombatantPrefab("test-monster", default, new FieldCoords(default, default));

        var sut = new MonsterPerkManager(dice, monsterPerkCatalog,
            globeProvider);

        // ACT

        var perks = sut.RollMonsterPerks(monster);

        // ASSERT

        perks.Single().Sid.Should().NotBe(blackPerk.Sid);
    }

    [Test]
    public void RollMonsterPerks_rolls_perks_only_for_black()
    {
        // ARRANGE

        var blackPerk = new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "black")
        {
            Predicates = new[]
            {
                new OnlyForBlackConclaveMonsterPerkPredicate()
            }
        };

        var regularPerk = new MonsterPerk(Mock.Of<ICombatantStatusFactory>(), "regular");

        var monsterPerkCatalog = Mock.Of<IMonsterPerkCatalog>(x => x.Perks == new[]
        {
            blackPerk,
            regularPerk
        });

        var diceMock = new Mock<IDice>();
        diceMock.SetupSequence(x => x.Roll(It.IsAny<int>()))
            .Returns(2) // to roll only single perk (n-1+min, where min == 0, totally == 1 on d2)
            .Returns(1);
        var dice = diceMock.Object;

        var globeProvider = new GlobeProvider(Mock.Of<ICharacterCatalog>(), Mock.Of<IStoryPointInitializer>(),
            monsterPerkCatalog);

        globeProvider.GenerateNew();

        globeProvider.Globe.Player.AddMonsterPerk(blackPerk);
        globeProvider.Globe.Player.AddMonsterPerk(regularPerk);
        globeProvider.Globe.Features.AddFeature(GameFeatures.UseMonsterPerks);

        var monster = new MonsterCombatantPrefab("aggressor", default, new FieldCoords(default, default));

        var sut = new MonsterPerkManager(dice, monsterPerkCatalog,
            globeProvider);

        // ACT

        var perks = sut.RollMonsterPerks(monster);

        // ASSERT

        perks.Single().Sid.Should().Be(blackPerk.Sid);
    }
}