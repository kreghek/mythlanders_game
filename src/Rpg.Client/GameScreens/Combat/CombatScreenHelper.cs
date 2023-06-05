using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat
{
    internal static class CombatScreenHelper
    {
        public static CombatRewards CalculateRewards(RewardCalculationContext context)
        {
            var combatSequenceXpBonuses = UnsortedHelpers.GetCombatSequenceXpBonuses();

            var monsters = context.CombatInfos.SelectMany(x => x.Monsters).ToArray();

            var sequenceBonus = combatSequenceXpBonuses[context.CombatInfos.Count() - 1];
            var summaryXp = (int)Math.Round(monsters.Sum(x => x.XpReward) * sequenceBonus);

            var rewardList = new List<ResourceReward>();
            if (context.EquipmentResourceDrop is not null)
            {
                var rewardItem = CreateReward(context.Inventory, context.EquipmentResourceDrop.Value, amount: 1);
                rewardList.Add(rewardItem);
            }

            var gainedXp = summaryXp;
            var xpRewardItem = CreateXpReward(context.Inventory, gainedXp);
            rewardList.Add(xpRewardItem);

            var combatRewards = new CombatRewards
            {
                BiomeProgress = new ResourceReward
                {
                    StartValue = context.BiomeCurrentLevel,
                    Amount = 1
                },
                InventoryRewards = rewardList
            };

            return combatRewards;
        }

        private static ResourceReward CreateReward(IEnumerable<ResourceItem> inventory,
            EquipmentItemType resourceType, int amount)
        {
            var currentAmount = 0;

            var inventoryItem = inventory.SingleOrDefault(x => x.Type == resourceType);
            if (inventoryItem is not null)
            {
                currentAmount = inventoryItem.Amount;
            }

            var item = new ResourceReward
            {
                StartValue = currentAmount,
                Amount = amount,
                Type = resourceType
            };

            return item;
        }

        private static ResourceReward CreateXpReward(IEnumerable<ResourceItem> inventory, int amount)
        {
            const EquipmentItemType EXPERIENCE_POINTS_TYPE = EquipmentItemType.ExperiencePoints;
            return CreateReward(inventory, EXPERIENCE_POINTS_TYPE, amount);
        }
    }
}