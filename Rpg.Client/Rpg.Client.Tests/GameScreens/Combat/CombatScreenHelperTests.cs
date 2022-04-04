using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Tests.GameScreens.Combat
{
    [TestFixture]
    public class CombatScreenHelperTests
    {
        [Test]
        public void CalculateRewards_HasEquipmentDrop_Returns1ItemOfEquipmentReward()
        {
            // ARRANGE

            const EquipmentItemType EQUIPMENT_RESOURCE_DROP = EquipmentItemType.Warrior;
            var context = new RewardCalculationContext(new List<ResourceItem>(0), EQUIPMENT_RESOURCE_DROP,
                new[] { new CombatRewardInfo(new[] { new CombatMonsterRewardInfo(default) }) }, default, default);

            // ACT

            var rewards = CombatScreenHelper.CalculateRewards(context);

            // ASSERT

            rewards.InventoryRewards.Single(x => x.Type == EQUIPMENT_RESOURCE_DROP).Amount.Should().Be(1);
        }
    }
}