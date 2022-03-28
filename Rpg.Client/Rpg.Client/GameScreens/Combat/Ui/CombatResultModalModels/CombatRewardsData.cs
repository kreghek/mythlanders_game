using System.Collections.Generic;

namespace Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels
{
    internal sealed class CombatRewardsData
    {
        public CombatRewardsData(AnimatedCountableUnitItemStat biomeProgress,
            IReadOnlyCollection<AnimatedCountableUnitItemStat> rewardItems)
        {
            BiomeProgress = biomeProgress;
            RewardItems = rewardItems;
        }

        public AnimatedCountableUnitItemStat BiomeProgress { get; }
        public IReadOnlyCollection<AnimatedCountableUnitItemStat> RewardItems { get; }

        public void Update()
        {
            BiomeProgress.Update();

            UpdateUnitItems();
        }

        private void UpdateUnitItems()
        {
            foreach (var unitItem in RewardItems)
            {
                unitItem.Update();
            }
        }
    }
}