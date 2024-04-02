using System.Linq;

using Client.Assets.Catalogs;
using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Dices;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Client.Tests.Assets;

public class MonsterPerkManagerTests
{
    [Test]
    public void RollLocationRewardPerks_do_not_roll_perks_only_for_black_for_other()
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
        
        var monsterPerkCatalog = Mock.Of<IMonsterPerkCatalog>(x=>x.Perks == new []
        {
            blackPerk,
            regularPerk
        });

        var dice = Mock.Of<IDice>(x => x.Roll(It.IsAny<int>()) == 0);

        var globeProvider = new GlobeProvider(It.IsAny<ICharacterCatalog>(), It.IsAny<IStoryPointInitializer>(),
            monsterPerkCatalog);
        
        globeProvider.GenerateNew();
        
        var sut = new MonsterPerkManager(dice, monsterPerkCatalog,
            globeProvider);
        
        // ACT

        var perks = sut.RollLocationRewardPerks();
        
        // ASSERT

        perks.Single().Sid.Should().Be(regularPerk.Sid);
    }
}